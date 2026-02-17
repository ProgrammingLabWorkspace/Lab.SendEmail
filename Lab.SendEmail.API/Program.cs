using Lab.SendEmail.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddAPI();

builder.AddInfra();

var app = builder.Build();

app.UseAPI();

app.Run();
