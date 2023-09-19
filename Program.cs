using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniPlanner.Areas.Identity.Data;

namespace UniPlanner
{ 
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("UniPlannerContextConnection") ?? throw new InvalidOperationException("Connection string 'UniPlannerContextConnection' not found.");

            builder.Services.AddDbContext<UniPlannerContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<UniPlannerUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<UniPlannerContext>();

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

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Manager", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UniPlannerUser>>();

                string FirstName = "Admin";
                string LastName = "UniPlanner";
                string email = "admin@uniplanner.com";
                string password = "uniPlanner0!";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new UniPlannerUser();
                    user.UserName = email;
                    user.Email = email;
                    user.FirstName = FirstName;
                    user.LastName = LastName;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }

            }
            app.Run();
        }
    }
}