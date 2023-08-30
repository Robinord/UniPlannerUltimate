using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniPlanner.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UniPlannerContextConnection") ?? throw new InvalidOperationException("Connection string 'UniPlannerContextConnection' not found.");

builder.Services.AddDbContext<UniPlannerContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<UniPlannerUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<UniPlannerContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
