#region Builder

using Microsoft.EntityFrameworkCore;
using RareForum.Models;
using RareForum.Static;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// This will handle simple auth. DO NOT USE THIS IS REAL APP!
builder.Services.AddSingleton<CAuth>();
// Connect to the database
builder.Services.AddDbContext<ForumDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ForumDB")));

// Add services to the container.
builder.Services.AddControllersWithViews();

#endregion
#region App
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// .net9 ting? 
// app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
// .net9 .WithStaticAssets()

app.Run();
#endregion