# SintLeoPannenkoeken

## Operations

### Deployment

Deployment happens by GitHub Actions after pushing `master` to GitHub. The backend is deployed to **both** Azure App Service and Scaleway Serverless Containers in parallel (see below); the database remains Azure SQL for both.

### Scaleway hosting

The Blazor backend also deploys to **Scaleway Serverless Containers**, alongside the existing Azure App Service deployment. Both run from the same `master` branch and the same Azure SQL database. This dual-deployment setup lets the Scaleway environment be validated in parallel without disrupting the Azure production deployment; Azure can still be considered the fallback if needed.

- Container image source: `SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor/Dockerfile`.
- Database: Azure SQL (unchanged; no schema/provider change for the Scaleway deployment — a separate database migration phase is planned for later).

Local container build:

```
docker build -f ./SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor/Dockerfile -t sintleopannenkoeken-blazor:local .
```

Local container run:

```
docker run --rm -p 8080:8080 -e ConnectionStrings__DefaultConnection="<existing sql server connection string>" sintleopannenkoeken-blazor:local
```

Scaleway resources:
- Scaleway Project ID: `da914e59-fe3d-4b7d-a9fc-0f6bec6a7fec`
- Scaleway Region: `fr-par`
- Scaleway Container Registry namespace: `sintleozeescouts`
- Scaleway Container Namespace ID: `242838a4-f8ed-4840-8351-e4c31e88641e`
- Scaleway Container: name `pannenkoeken`, ID `5e3bbf99-6f0b-4018-8c26-1fa77c5c4312`
- Container resources: 512MB memory / 250mVCPU (see "container OOM" note below for why this matters)
- Azure SQL firewall: currently open to all IPs (temporary; to be replaced once the database itself migrates to Scaleway, see "Deliverables"/backlog below)

#### Secrets and configuration

- Container registry: **Scaleway Container Registry**.
- Secrets: **plain container environment variables** set on the Scaleway Serverless Container (no Secret Manager).
- The existing `ConnectionStrings__DefaultConnection` contract is unchanged, so no application code changes were required for config plumbing.

**GitHub Actions repository secrets** (sensitive; Settings → Secrets and variables → Actions → Secrets):

| Secret name | Purpose |
| --- | --- |
| `SCW_ACCESS_KEY` | Scaleway API access key |
| `SCW_SECRET_KEY` | Scaleway API secret key |
| `SCW_DEFAULT_PROJECT_ID` | Scaleway Project ID |
| `SCW_DEFAULT_ORGANIZATION_ID` | Scaleway Organization ID (required by the `scw` CLI in addition to the Project ID — found in the Scaleway console under Organization settings) |

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

Manual image tagging/push for the Scaleway Container Registry (if needed outside CI):

```
echo "<SCW_SECRET_KEY>" | docker login rg.fr-par.scw.cloud/sintleozeescouts -u nologin --password-stdin
docker tag sintleopannenkoeken-blazor:local rg.fr-par.scw.cloud/sintleozeescouts/sintleopannenkoeken-blazor:local
docker push rg.fr-par.scw.cloud/sintleozeescouts/sintleopannenkoeken-blazor:local
```

#### CI/CD

A dedicated workflow, `.github/workflows/scaleway-deploy.yml`, builds, tests, pushes the backend image to the Scaleway Container Registry, and deploys it to the `pannenkoeken` Serverless Container whenever `master` is pushed. This runs independently and in parallel with `.github/workflows/SintLeoPannenkoeken.yml`, which deploys `master` to Azure.

- `build_and_push_image` job: builds, tests, and pushes `sintleopannenkoeken-blazor:<git-sha>` and `:latest` to `rg.fr-par.scw.cloud/sintleozeescouts`.
- `deploy_container` job: updates the `pannenkoeken` container (`SCW_CONTAINER_ID`) with the new image tag via the Scaleway CLI (`scaleway/action-scw@v0`); updating the image triggers an automatic redeploy.
- The deploy step sets `http-option=redirected`, so Scaleway redirects public HTTP requests to HTTPS at the edge.
- Required repository secrets/variables (see tables above) must all be set for this workflow to succeed end to end.

#### Custom domain and HTTPS

HTTPS enforcement for the Scaleway deployment happens at the Scaleway gateway, not only inside ASP.NET Core. The container listens on plain HTTP (`8080`) behind Scaleway's TLS-terminating gateway, and the GitHub Actions deploy step configures the container with `http-option=redirected` so public HTTP traffic is redirected to HTTPS.

To put the app behind a real domain:
1. Register the domain with a registrar/DNS provider.
2. Add a DNS record pointing the desired hostname to the default Scaleway container endpoint:
   - For a subdomain such as `app.example.com`: create a `CNAME` to `sintleozeescouts242838a4-pannenkoeken.functions.fnc.fr-par.scw.cloud`.
   - For a root/apex domain such as `example.com`: use CNAME flattening or an `ALIAS`/`ANAME` record if the DNS provider supports it.
3. In Scaleway, open the `pannenkoeken` container, go to **Endpoints**, and add the custom domain.
4. Wait until Scaleway marks the endpoint as ready; Scaleway provisions the TLS certificate automatically via HTTP-01 challenge.
5. Test both `http://<domain>` and `https://<domain>`; HTTP should redirect to HTTPS.

ASP.NET Core `UseHttpsRedirection()`/HSTS can remain as secondary safeguards, but the primary HTTPS enforcement for Scaleway is the gateway-level `http-option=redirected` setting.

#### DataProtection keys (why they're stored in the database)

ASP.NET Core's DataProtection key ring defaults to local disk. On Azure App Service this worked transparently because `%HOME%` is persistent, shared storage across restarts/instances. Scaleway Serverless Container instances have **ephemeral, per-instance local disk** — every redeploy/restart/scale event wipes the key ring, invalidating antiforgery tokens and Blazor Server circuit tokens issued by a previous instance (symptom: browser shows "Rejoining the server" / `CryptographicException: key not found in key ring`).

**Fix**: DataProtection keys are persisted to the Azure SQL database instead of local disk:
- `ApplicationDbContext` implements `IDataProtectionKeyContext` and exposes a `DataProtectionKeys` `DbSet`.
- `Program.cs` registers `AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>()`.
- Package reference `Microsoft.AspNetCore.DataProtection.EntityFrameworkCore` added.
- EF Core migration `AddDataProtectionKeys` creates the table (already applied to the Azure SQL database in use).

This only matters for the Scaleway deployment; Azure App Service continues to work as before regardless.

#### Container sizing note

Scaleway container resources were initially too low (128MB memory / 100mVCPU), causing intermittent OOM kills at startup with no exception logged (symptom: Scaleway reports "Container is unable to start OR is not listening on port 8080", app logs stop right after a DB query). Resolved by increasing resources to **512MB memory / 250mVCPU**. Memory is generally the more important dimension to size for stability (see incident runbook below); increase CPU if throughput/latency issues appear under load instead.

#### CORS and health checks (confirmed non-issues)

- **CORS**: the `AllowBlazorWasm` policy hardcoding `https://localhost:64389` doesn't need to change for Scaleway. CORS only applies to cross-origin requests, and this app serves its UI and API from the same origin on both Azure and Scaleway, so the browser never triggers a CORS check.
- **Health endpoints**: Scaleway Serverless Containers determine readiness purely by whether the container is listening on its configured port — no HTTP health-check path is required ([Scaleway docs](https://www.scaleway.com/en/docs/serverless-containers/concepts/#cold-start)). Keeping `/health`/`/alive` gated behind `IsDevelopment()` is fine as-is.

#### Operational runbook

Telemetry: the Scaleway console's built-in **Logs** and **Metrics** tabs are used for this deployment (no external OTLP export configured).

**Rollback**
1. Find the previous known-good image tag (git SHA) — either from a prior successful `build_and_push_image` run, or by listing tags in the Scaleway Container Registry.
2. Redeploy that tag manually:
   ```
   scw container container update <SCW_CONTAINER_ID> region=fr-par registry-image=rg.fr-par.scw.cloud/sintleozeescouts/sintleopannenkoeken-blazor:<previous-sha> redeploy=true
   ```
   (or set the same value in the Scaleway console under the container's configuration and redeploy).
3. Confirm the container's Logs tab shows the rolled-back instance starting cleanly and the app loads correctly.

**Incident triage checklist**
- Check the container's **status** in the Scaleway console (e.g. stuck on "Updating" can mean the new revision failed to start and the previous revision is still serving traffic).
- Check the **Logs** tab for the failing instance's ID specifically — a normal boot logs `Now listening on: http://[::]:8080` and `Application started`; if logs stop abruptly with no exception right after a DB query, suspect an **OOM kill** (increase memory/CPU limits).
- Check **memory/CPU usage graphs** in Metrics if scaling limits are suspected.
- If antiforgery/`CryptographicException`/"Rejoining the server" errors reappear, confirm the `DataProtectionKeys` table still exists and the app can reach Azure SQL (firewall rules, connection string env var).
- As a last resort, redeploy the previous image tag per the rollback steps above.

#### Known backlog / deferred items

- **Database migration**: SQL Server (Azure SQL) → a Scaleway-hosted database is planned as a separate, later phase. The Azure SQL firewall is deliberately left open to all IPs until then; it will be replaced by Scaleway-native networking once the database itself moves.

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
