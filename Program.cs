using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddFastEndpoints();
app.UseFastEndpoints();
app.Run();
