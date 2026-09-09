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

Scaleway runtime inputs to confirm before enabling deployment pipeline:
- Scaleway Project ID
- Scaleway Region
- Scaleway Container Namespace
- Scaleway Container Name
- Test domain and TLS certificate source
- SQL Server firewall/allowlist for Scaleway egress

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
