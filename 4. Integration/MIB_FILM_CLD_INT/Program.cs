using MIB_FILM_CLD_INT.Infrastructure;
using MIB_FILM_CLD_INT.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ConnectionStringFileProvider>();
builder.Services.AddSingleton<LegacyIntegrationDb>();
builder.Services.AddScoped<SelfEfficiencyIntegrationService>();
builder.Services.AddScoped<StockControlIntegrationService>();
builder.Services.AddScoped<ProductivityIntegrationService>();
builder.Services.AddScoped<OprRatioIntegrationService>();
builder.Services.AddScoped<SalesOrderIntegrationService>();

var app = builder.Build();

app.MapGet("/FILM_LMI_SELF_EFF_4DAYS_INT.aspx", (SelfEfficiencyIntegrationService service) =>
{
    service.Run4Days();
    return Results.Content(LegacyHtmlPage.Create("FILM LMI SELF EFF 4DAYS INTEGRATION"), "text/html");
});

app.MapGet("/FILM_LMI_SELF_EFF_YTD_INT.aspx", (SelfEfficiencyIntegrationService service) =>
{
    service.RunYtd();
    return Results.Content(LegacyHtmlPage.Create("FILM LMI SELF EFF YTD INTEGRATION"), "text/html");
});

app.MapGet("/FILM_LMI_STOCK_CONTROL_INT.aspx", (StockControlIntegrationService service) =>
{
    service.Run();
    return Results.Content(LegacyHtmlPage.Create("FILM LMI STOCK CONTROL INTEGRATION"), "text/html");
});

app.MapGet("/FILM_LMI_PRODUCTIVITY_INT.aspx", (ProductivityIntegrationService service) =>
{
    service.Run();
    return Results.Content(LegacyHtmlPage.Create("FILM LMI PRODUCTIVITY INTEGRATION"), "text/html");
});

app.MapGet("/FILM_LMI_OPR_RATIO_INT.aspx", (OprRatioIntegrationService service) =>
{
    service.Run();
    return Results.Content(LegacyHtmlPage.Create("FILM LMI OPR RATIO"), "text/html");
});

app.MapGet("/FILM_LMI_SALES_ORDER_INT.aspx", (SalesOrderIntegrationService service) =>
{
    service.Run();
    return Results.Content(LegacyHtmlPage.Create("FILM LMI SALES ORDER"), "text/html");
});

app.Run();
