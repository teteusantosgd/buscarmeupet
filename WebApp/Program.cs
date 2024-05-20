using WebApp.Data;
using WebApp.Data.Repositories;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
        config.AddCommandLine(args);
    })
    .Build();

var config = host.Services.GetRequiredService<IConfiguration>();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

string database = config.GetSection("ConnectionStrings:Database")?.Value;
string connection = config.GetSection("ConnectionStrings:Mongo")?.Value;
builder.Services.AddTransient(x => new AppDbContext(database, connection));

builder.Services.AddScoped<IPetsRepository, PetsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
