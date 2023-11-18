var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DncyTemplate_Api>("dncytemplate.api");

builder.Build().Run();
