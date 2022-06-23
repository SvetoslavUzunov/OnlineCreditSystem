using Microsoft.EntityFrameworkCore;
using OnlineCreditSystem.Data;
using OnlineCreditSystem.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var connection = @"Server=(localdb)\mssqllocaldb;Database=OnlineCreditSystem;Trusted_Connection=True;MultipleActiveResultSets=true";
builder.Services.AddDbContext<OnlineCreditSystemDbContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.PrepareDatabase();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
