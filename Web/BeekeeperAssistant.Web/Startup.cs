namespace BeekeeperAssistant.Web
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using BeekeeperAssistant.Data;
    using BeekeeperAssistant.Data.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Data.Repositories;
    using BeekeeperAssistant.Data.Seeding;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Services.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Services.Messaging;
    using BeekeeperAssistant.Web.Filters;
    using BeekeeperAssistant.Web.Infrastructure.RouteConstraints;
    using BeekeeperAssistant.Web.Routes;
    using BeekeeperAssistant.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                        options.Filters.Add(new RedirectResourceFilter());
                    }).AddRazorRuntimeCompilation();

            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("apiaryNumber", typeof(ApiaryNumberRouteConstraint));
            });

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(this.configuration["SendGrid:ApiId"]));
            services.AddTransient<IApiaryService, ApiaryService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IForecastService, ForecastService>();
            services.AddTransient<IQuickChartService, QuickChartService>();
            services.AddTransient<IApiaryNumberService, ApiaryNumberService>();
            services.AddTransient<IBeehiveService, BeehiveService>();
            services.AddTransient<IQueenService, QueenService>();
            services.AddTransient<IHarvestService, HarvestService>();
            services.AddTransient<ITreatmentService, TreatmentService>();
            services.AddTransient<IInspectionService, InspectionService>();
            services.AddTransient<IApiaryHelperService, ApiaryHelperService>();
            services.AddTransient<IEnumerationMethodsService, EnumerationMethodsService>();
            services.AddTransient<IBeehiveHelperService, BeehiveHelperService>();
            services.AddTransient<IQueenHelperService, QueenHelperService>();
            services.AddTransient<IExcelExportService, ExcelExportService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IStringManipulationService, StringManipulationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ApiaryDataExcelExportModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/HttpError", "?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.ConfigureBeekeeperAssistantRoutes();
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
