var builder = DistributedApplication.CreateBuilder(args);


#if (ProjectType == "API")
builder.AddProject<Projects.DncyTemplate_Api>("dncytemplate.api");
#endif

#if (ProjectType == "MVC")
builder.AddProject<Projects.DncyTemplate_Mvc>("dncytemplate.mvc");
#endif

builder.Build().Run();
