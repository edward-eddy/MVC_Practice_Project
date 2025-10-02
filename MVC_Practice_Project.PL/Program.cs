using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.BLL.Repositories;
using MVC_Practice_Project.DAL.Data.Contexts;
using MVC_Practice_Project.PL.Mapping;
using MVC_Practice_Project.PL.Services;

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

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow DI For DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Allow DI For DepartmentRepository
            //builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new EmployeeProfile()); });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                //options.UseSqlServer(builder.Configuration["DefaultConnection"]);
            }/*, ServiceLifetime.Singleton*/); // Allow DI For AppDbContext

            //Life Time
            //builder.Services.AddScoped();     // Create Object Life Time Per Request - Unreachable Object
            //builder.Services.AddTransient();  // Create Object Life Time Per Operation
            //builder.Services.AddSingleton();  // Create Object Life Time Per App

            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();

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
