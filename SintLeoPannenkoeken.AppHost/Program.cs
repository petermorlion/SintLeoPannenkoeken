var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sqlserver")
                 .WithLifetime(ContainerLifetime.Persistent)
                 .WithDataVolume("sqlserver-data")
                 .WithHostPort(61847);

var db = sql.AddDatabase("database");

var app = builder.AddProject<Projects.SintLeoPannenkoeken_Blazor>("sintleopannenkoeken-blazor")
       .WithReference(db)
       .WaitFor(db);

var migrations = app.AddEFMigrations("app-migrations")
    .RunDatabaseUpdateOnStart()
    .WaitFor(db);

app.WaitForCompletion(migrations);

builder.Build().Run();
