using Microsoft.Data.SqlClient;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging (avoid logging secrets)
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Security headers & TLS
app.UseHsts();
app.UseHttpsRedirection();
app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["X-Frame-Options"] = "DENY";
    h["Referrer-Policy"] = "no-referrer";
    h["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
    h["Content-Security-Policy"] = "default-src 'self'; frame-ancestors 'none'; base-uri 'self'";
    await next();
});

app.UseAuthorization();

app.MapControllers();

// Swagger (dev only by default; okay here since this is a mock)
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
