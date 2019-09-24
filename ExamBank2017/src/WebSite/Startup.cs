using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebSite.Data;
using WebSite.Models;
using WebSite.Services;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Driver;
using WebSite.Repositories;
using TheS.ExamBank.Parsers;

namespace WebSite
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<Data.ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<Data.ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            var webconfig = Configuration.GetSection("WebConfiguration").Get<WebConfiguration>();

            services.AddTransient<IWebConfiguration>(pvdr => webconfig);
            services.AddSingleton(pvdr => CloudStorageAccount.Parse(webconfig.StorageConnectionString));
            services.AddTransient(pvdr => new MongoClient(webconfig.MongoDbConnectionString));
            services.AddTransient<Repositories.MongoImpl.MongoHelper>();
            services.AddTransient<IQuestionImportRepository, Repositories.OracleImpl.OraQuestionImportRepository>();
            services.AddTransient<IExamForApproveRepository, Repositories.OracleImpl.OraExamForApproveRepository>();
            //services.AddTransient<IQuestionImportRepository, Repositories.MongoImpl.QuestionImportRepository>();
            //services.AddTransient<IExamForApproveRepository, Repositories.MongoImpl.ExamForApproveRepository>();
            services.AddTransient<IExamForRandomRepository, Repositories.MongoImpl.ExamForRandomRepository>();
            services.AddTransient<ICloudStorage, Repositories.MongoImpl.CloudStorage>();
            services.AddTransient<IFile, TheS.ExamBank.Parsers.FileHelper>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
