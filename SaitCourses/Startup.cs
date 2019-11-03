using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SaitCourses.Models;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using Localization.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using SaitCourses.ViewModels;


namespace SaitCourses
{
    public class Startup
    {
        private readonly UserManager<User> _userManager;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    if (await roleManager.FindByNameAsync("Admin") == null)
        //    {
        //        await roleManager.CreateAsync(new IdentityRole("Admin"));
        //    }
        //    if (await roleManager.FindByNameAsync("User") == null)
        //    {
        //        await roleManager.CreateAsync(new IdentityRole("User"));
        //    }
        //}


        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
            services.Configure<RequestLocalizationOptions>(async options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

            });
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

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
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<CommentsHub>("/Comments");
            });

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            //IList<CultureInfo> supportedCultures = new List<CultureInfo>
            //{
            //     new CultureInfo("en"),
            //     new CultureInfo("ru"),
            //};
            //var localizationOptions = new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("en"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //};
            //var requestProvider = new RouteDataRequestCultureProvider();
            //localizationOptions.RequestCultureProviders.Insert(0, requestProvider);

            //app.UseRouter(routes =>
            //{
            //    routes.MapMiddlewareRoute("{culture=en-US}/{*mvcRoute}", subApp =>
            //    {
            //        subApp.UseRequestLocalization(localizationOptions);

            app.UseMvc(mvcRoutes =>
            {
                mvcRoutes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
