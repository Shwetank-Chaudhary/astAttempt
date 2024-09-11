using Microsoft.AspNetCore.Authentication.Cookies;
using astAttempt.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using astAttempt.Contracts;
namespace astAttempt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));
            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add services to the container.
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/AccAdmin/LoginAdmin"; // Default login path
                    options.LogoutPath = "/AccAdmin/Logout"; // Default logout path
                    options.AccessDeniedPath = "/AccAdmin/AccessDenied"; // Access denied path
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
                options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));// Add a policy for employees if needed
            });






            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();


            //builder.Services.AddAuthentication("CustomAuthScheme")
            //    .AddScheme<AuthenticationSchemeOptions, CustomAuthHandler>("CustomAuthScheme", null);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>)); // Repository configuration
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Enable authentication
            app.UseAuthorization();  // Enable authorization

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Map("/AccessDenied", (context) =>
            {
                context.Response.Redirect("/Shared/AccessDenied");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}
