using Calabonga.AspNetCore.AppDefinitions;

var builder = WebApplication.CreateBuilder(args);

// gateway

builder.Services.AddDefinitions(builder, typeof(Program));

// create application
var app = builder.Build();

// using definition for application
app.UseDefinitions();

// start application
app.Run();
