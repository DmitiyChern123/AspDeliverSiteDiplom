using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;
using WebApplication1.Entities2;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Log.Logger = new LoggerConfiguration()
         .WriteTo.File(
             formatter: new JsonFormatter(),
             path: "log.json",
             rollingInterval: RollingInterval.Day,
             shared:true,
             flushToDiskInterval: TimeSpan.FromSeconds(1)
         )
         .CreateLogger();
          //  builder.Services.AddDbContext<DiplomContext>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/Login";

            });

            builder.Services.AddDbContext<DiplomContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(5);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });
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
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            ///


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        

        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}