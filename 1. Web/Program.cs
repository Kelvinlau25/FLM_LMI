using DatabaseModel;
using DBModel;

var builder = WebApplication.CreateBuilder(args);

// Startup validation for required configuration values
_ = builder.Configuration.GetConnectionString("PAB_BB")
    ?? throw new InvalidOperationException("Connection string 'PAB_BB' is not configured in appsettings.json");

_ = builder.Configuration.GetConnectionString("DBAccess")
    ?? throw new InvalidOperationException("Connection string 'DBAccess' is not configured in appsettings.json");

_ = builder.Configuration["AppSettings:SystemName"]
    ?? throw new InvalidOperationException("'AppSettings:SystemName' is not configured in appsettings.json");

// Inject configuration into the data layer
DatabaseModel.Database.Configure(builder.Configuration);
ACLDatabase.Configure(builder.Configuration);
DBModel.Database.Configure(builder.Configuration);

// Services
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
