using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChartJsCulture.Data;
using static ChartJsCulture.Pages.Index;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ChartJsCulture
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllers();
            services.AddSingleton<WeatherForecastService>();

            Language[] languages =  {
                new Language
                {
                    LangId = "en-US",
                    Title = "English"
                },
                new Language
                {
                    LangId = "it-IT",
                    Title = "Italiano"
                }
            };

            CultureInfo[] supportedCultures = new CultureInfo[languages.Length];

            for (int i = 0; i < languages.Length; i++)
            {
                supportedCultures[i] = new CultureInfo(languages[i].LangId);
            }

            RequestCulture requestCulture = new RequestCulture("en-US");

            IRequestCultureProvider[] requestCultureProvider = new IRequestCultureProvider[]
            {
                new CookieRequestCultureProvider()
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.RequestCultureProviders = requestCultureProvider;
                options.DefaultRequestCulture = requestCulture;
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
