var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SintLeoPannenkoeken_Blazor>("sintleopannenkoeken-blazor");

builder.Build().Run();
