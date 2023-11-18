var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DncyTemplate_Api>("dncytemplate.api");

builder.AddProject<Projects.DncyTemplate_Mvc>("dncytemplate.mvc");

builder.Build().Run();
