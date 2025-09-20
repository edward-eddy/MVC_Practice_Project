using Microsoft.EntityFrameworkCore;
using MVC_Practice_Project.BLL.Repositories;
using MVC_Practice_Project.DAL.Data.Contexts;

namespace MVC_Practice_Project.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Register Built-in MVC Services

            //builder.Services.AddTransient();
            //builder.Services.AddSingleton();

            builder.Services.AddScoped<DepartmentRepository>(); // Allow DI For DepartmentRepository

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("Server = .; Database = MVC_PP_DB; Trusted_Connection = True; TrustServerCertificate = True");
            }); // Allow DI For AppDbContext

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

            //app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
