using Calabonga.AspNetCore.AppDefinitions;

// contacts

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefinitions(builder, typeof(Program));

// create application
var app = builder.Build();

// using definition for application
app.UseDefinitions();

// start application
app.Run();