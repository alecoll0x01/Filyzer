var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Filyzer>("filyzer");

builder.Build().Run();
