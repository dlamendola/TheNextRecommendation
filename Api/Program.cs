using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Handler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapRoutes();

app.Run();