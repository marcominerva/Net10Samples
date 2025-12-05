#!/usr/bin/env dotnet

#:sdk Microsoft.NET.Sdk.Web

#:property TargetFramework=net10.0
#:property PublishAot=false

#:package Microsoft.AspNetCore.OpenApi@10.0.*
#:package Swashbuckle.AspNetCore.SwaggerUI@10.0.*

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options => 
{
	options.SwaggerEndpoint("/openapi/v1.json", app.Environment.ApplicationName);
	options.RoutePrefix = string.Empty;
});

app.MapGet("/api/greetings", (string? name) => new GreetingResponse($"Hello, {name ?? "World"}!"));

app.Run();

public record GreetingResponse(string Message);