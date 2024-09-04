using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Context;
using StudentManagementSystem.Services;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace StudentManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context, config) =>
                    {
                        var env = context.HostingEnvironment;
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                              .AddEnvironmentVariables();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        var configuration = context.Configuration;

                        // Add controllers with views and configure Newtonsoft.Json for handling circular references
                        services.AddControllersWithViews()
                            .AddNewtonsoftJson(options =>
                            {
                                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                            });

                        // Register ApplicationDbContext
                        services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

                        // Register StudentContext
                        services.AddDbContext<StudentContext>(options =>
                            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

                        // Register IFingerprintProSdk with concrete implementation
                        services.AddScoped<IFingerprintProSdk, FingerprintProSdkImplementation>();

                        // Register FingerprintService
                        services.AddScoped<FingerprintService>();

                        // Add Swagger
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo
                            {
                                Version = "v1",
                                Title = "Student Management System API",
                                Description = "An API for managing student data",
                                Contact = new OpenApiContact
                                {
                                    Name = "Your Name",
                                    Email = string.Empty,
                                    Url = new Uri("https://example.com"),
                                }
                            });
                        });
                    })
                    .Configure((context, app) =>
                    {
                        var env = context.HostingEnvironment;

                        if (env.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }
                        else
                        {
                            app.UseExceptionHandler("/Home/Error");
                            app.UseHsts();
                        }

                        app.UseHttpsRedirection();
                        app.UseStaticFiles();

                        app.UseRouting();

                        app.UseAuthorization();

                        // Enable middleware to serve generated Swagger as a JSON endpoint
                        app.UseSwagger();

                        // Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.),
                        // specifying the Swagger JSON endpoint.
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management System API V1");
                            c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
                        });

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}");
                        });
                    });
                });
    }
}
