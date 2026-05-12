using DBConnection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MIB_FILM_CLD.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Initialize connection string from configuration
ConnectionString.FILM_CLD = builder.Configuration.GetConnectionString("FILM_CLD");

var app = builder.Build();

// Register endpoints with exact legacy paths
app.MapGet("/JSON_FILM_CLD.ashx", JsonFilmEndpoint.HandleRequest);
app.MapGet("/JSON_EMAIL_FILM_CLD.ashx", JsonEmailEndpoint.HandleRequest);
app.MapGet("/VERIFY_FILM_CLD.aspx", VerifyEndpoint.HandleRequest);

// Root redirect to verification page
app.MapGet("/", () => Results.Redirect("/VERIFY_FILM_CLD.aspx"));

app.Run();
