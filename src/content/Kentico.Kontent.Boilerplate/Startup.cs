using System;
using Kentico.Kontent.Delivery.Caching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kentico.Kontent.Delivery.Caching.Extensions;
using Kentico.Kontent.AspNetCore.ImageTransformation;
using KenticoKontentModels;
using Kentico.AspNetCore.LocalizedRouting.Extensions;
using Kentico.Kontent.Boilerplate.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Kentico.Kontent.Boilerplate.Configuration;

namespace Kentico.Kontent.Boilerplate
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds services required for using options.
            services.AddOptions();

            // Register the ImageTransformationOptions required by Kentico Kontent tag helpers
            services.Configure<ImageTransformationOptions>(Configuration.GetSection(nameof(ImageTransformationOptions)));

            services.AddSingleton<CustomTypeProvider>();
            //services.AddSingleton<CustomContentLinkUrlResolver>();

            // Navigation
            services.AddMemoryCache();
            services.AddSingleton<NavigationProvider>();


            // I18N
            services.ConfigureRequestLocalization(CultureConstants.DefaultCulture, CultureConstants.SpanishCulture);
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<CustomLocalizedRoutingTranslationTransformer>();
            services.AddControllersWithViews();
            services.AddLocalizedRouting(); 

            services.AddDeliveryClient(Configuration);

            // Use cached client decorator
            services.AddDeliveryClientCache(new DeliveryCacheOptions()
            {
                StaleContentExpiration = TimeSpan.FromSeconds(2),
                DefaultExpiration = TimeSpan.FromMinutes(1)
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Add IIS URL Rewrite list, see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/url-rewriting
            var options = new RewriteOptions().AddIISUrlRewrite(env.ContentRootFileProvider, "IISUrlRewrite.xml");
            app.UseRewriter(options);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDynamicControllerRoute<CustomLocalizedRoutingTranslationTransformer>("{culture=en-US}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "cafes",
                            pattern: "{culture}/{controller}/{**id}",
                            defaults: new { controller = "Cafes", action = "Index" });
                endpoints.MapControllerRoute("default", "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
