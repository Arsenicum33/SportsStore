using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace SportsStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration["Data:SportsStoreProducts:ConnectionString"]));
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(
                Configuration["Data:SportsStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSession();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: null,
                template: "{category}/Page{productPage:int}",
                defaults: new { controller = "Product", action = "List" }
                );
                routes.MapRoute(
                name: null,
                template: "Page{productPage:int}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                }
                );
                routes.MapRoute(
                name: null,
                template: "Order/List",
                defaults: new
                {
                    controller = "Order",
                    action = "List",
                });
                routes.MapRoute(
                name: null,
                template: "{category}",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                }
                );
                routes.MapRoute(
                name: null,
                template: " ",
                defaults: new
                {
                    controller = "Product",
                    action = "List",
                    productPage = 1
                });
                routes.MapRoute(name: null, template: "{controller=Product}/{action=List}/{id?}");
            });


        }
    }
}
