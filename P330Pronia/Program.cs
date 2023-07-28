using Microsoft.EntityFrameworkCore;
using P330Pronia.Contexts;
using P330Pronia.Services.Implementations;
using P330Pronia.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    //options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});

builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}"
);

app.Run();
