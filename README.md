# SintLeoPannenkoeken

## Operations

### Deployment

Deployment happens by GitHub actions after pushing master to GitHub.

### Add a migration

```
dotnet ef migrations add <MigrationName> --project .\SintLeoPannenkoeken
```

### Update Azure database

```
dotnet ef database update --connection "Server=tcp:sintleo.database.windows.net,1433;Initial Catalog=SintLeoPannenkoeken;Persist Security Info=False;User ID=<user id>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" --project .\SintLeoPannenkoeken\SintLeoPannenkoeken.csproj
```
