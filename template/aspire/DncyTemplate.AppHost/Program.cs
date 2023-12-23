var builder = DistributedApplication.CreateBuilder(args);

#if IsAPI
builder.AddProject<Projects.DncyTemplate_Api>("dncytemplate.api");
#endif

#if IsMVC
builder.AddProject<Projects.DncyTemplate_Mvc>("dncytemplate.mvc");
#endif

builder.Build().Run();
