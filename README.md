# SintLeoPannenkoeken

## Operations

### Deployment

Deployment happens by GitHub actions after pushing master to GitHub.

### Scaleway hosting pilot (step 1)

The branch `feature/scaleway-hosting-step1` is used to prepare an isolated Scaleway deployment path while keeping Azure production deployment on `master`.

Current step-1 scope:
- Backend runtime target: **Scaleway Serverless Containers**.
- Database remains SQL Server (no migration in this phase).
- Container image source: `SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor/Dockerfile`.

Local container build:

```
docker build -f ./SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor/Dockerfile -t sintleopannenkoeken-blazor:scaleway-step1 .
```

Local container run:

```
docker run --rm -p 8080:8080 -e ConnectionStrings__DefaultConnection="<existing sql server connection string>" sintleopannenkoeken-blazor:scaleway-step1
```

Scaleway runtime inputs (confirmed so far):
- Scaleway Project ID: `da914e59-fe3d-4b7d-a9fc-0f6bec6a7fec`
- Scaleway Region: `fr-par`
- Scaleway Container Registry namespace: `sintleozeescouts`
- Scaleway Container Namespace ID: `242838a4-f8ed-4840-8351-e4c31e88641e`
- Scaleway Container: name `pannenkoeken`, ID `5e3bbf99-6f0b-4018-8c26-1fa77c5c4312`
- Test domain and TLS certificate source: not yet decided
- SQL Server firewall/allowlist for Scaleway egress: not yet validated

### Scaleway hosting pilot (step 2): secrets and configuration

Decisions for this phase:
- Container registry: **Scaleway Container Registry**.
- Secrets: **plain container environment variables** set on the Scaleway Serverless Container (no Secret Manager for this phase).
- The existing `ConnectionStrings__DefaultConnection` contract is unchanged, so no application code changes are required.

**Prerequisite (manual, one-time, done by a human, not automatable here):** create a Scaleway account, a Project, and an API key (access key + secret key) with Container Registry and Serverless Containers permissions, and create a Container Registry namespace for this app.

**GitHub Actions repository secrets** (sensitive; Settings → Secrets and variables → Actions → Secrets):

| Secret name | Purpose |
| --- | --- |
| `SCW_ACCESS_KEY` | Scaleway API access key |
| `SCW_SECRET_KEY` | Scaleway API secret key |
| `SCW_DEFAULT_PROJECT_ID` | Scaleway Project ID |

**GitHub Actions repository variables** (non-sensitive IDs/names; Settings → Secrets and variables → Actions → Variables):

| Variable name | Value |
| --- | --- |
| `SCW_DEFAULT_REGION` | `fr-par` |
| `SCW_REGISTRY_NAMESPACE` | `sintleozeescouts` |
| `SCW_CONTAINER_ID` | `5e3bbf99-6f0b-4018-8c26-1fa77c5c4312` (container `pannenkoeken`, in namespace `242838a4-f8ed-4840-8351-e4c31e88641e`) |

**Production runtime environment variables required on the Scaleway container** (set directly on the container, not baked into the image):

| Variable | Source today | Notes |
| --- | --- | --- |
| `ASPNETCORE_ENVIRONMENT` | n/a | Set to `Production` |
| `ASPNETCORE_URLS` | `Dockerfile` default `http://+:8080` | Only override if Scaleway requires a different port |
| `ConnectionStrings__DefaultConnection` | `appsettings.json` | Existing SQL Server connection string |
| `RouteXL__UserName` | `appsettings.json` (`RouteXL:UserName`) | RouteXL account username |
| `RouteXL__Password` | `appsettings.json` (`RouteXL:Password`) | RouteXL account password |
| `RouteXL__ApiKey` | `appsettings.json` (`RouteXL:ApiKey`) | RouteXL API key (optional, `HasApiKey`) |
| `HereApiKey` | Read via `Environment.GetEnvironmentVariable("HereApiKey")` in `HereGeocodingService`/`HereTourPlanningService` | HERE Geocoding/Tour Planning API key |

Local image tagging for the Scaleway Container Registry:

```
echo "<SCW_SECRET_KEY>" | docker login rg.fr-par.scw.cloud/sintleozeescouts -u nologin --password-stdin
docker tag sintleopannenkoeken-blazor:scaleway-step1 rg.fr-par.scw.cloud/sintleozeescouts/sintleopannenkoeken-blazor:scaleway-step1
docker push rg.fr-par.scw.cloud/sintleozeescouts/sintleopannenkoeken-blazor:scaleway-step1
```

### Scaleway hosting pilot (step 3): CI/CD build, push and deploy

A dedicated workflow, `.github/workflows/scaleway-deploy.yml`, builds, tests, pushes the backend image to the Scaleway Container Registry, and deploys it to the `pannenkoeken` Serverless Container whenever `feature/scaleway-hosting-step1` is pushed. This is fully separate from `.github/workflows/SintLeoPannenkoeken.yml`, which still deploys `master` to Azure unchanged.

Status:
- `build_and_push_image` job: builds, tests, and pushes `sintleopannenkoeken-blazor:<git-sha>` and `:latest` to `rg.fr-par.scw.cloud/sintleozeescouts`.
- `deploy_container` job: updates the `pannenkoeken` container (`SCW_CONTAINER_ID`) with the new image tag via the Scaleway CLI (`scaleway/action-scw@v0`); updating the image triggers an automatic redeploy.
- Required repository secrets/variables (see table above) must all be set before this workflow will succeed end to end.
- Not yet configured on the container itself: the production environment variables listed above (connection string, RouteXL/HERE keys) — set these once via the Scaleway console or CLI on the `pannenkoeken` container before the first real smoke test.

### Add a migration

Modify the ApplicationDbContextFactory to get the connection string from the `ConnectionStrings__DefaultConnection`
environment variable instead of `ConnectionStrings__database`.

```
dotnet ef migrations add <MigrationName> --project .\SintLeoPannenkoeken
```

Put back the original environment variable in the ApplicationDbContextFactory.

### Update Azure database

```
dotnet ef database update --connection "Server=tcp:sintleo.database.windows.net,1433;Initial Catalog=SintLeoPannenkoeken;Persist Security Info=False;User ID=<user id>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" --project .\SintLeoPannenkoeken\SintLeoPannenkoeken.csproj
```
