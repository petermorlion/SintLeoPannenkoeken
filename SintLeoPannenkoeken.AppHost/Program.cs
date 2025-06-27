var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sqlserver")
                 .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("database");

builder.AddProject<Projects.SintLeoPannenkoeken_Blazor>("sintleopannenkoeken-blazor")
       .WithReference(db)
       .WaitFor(db);

builder.Build().Run();
