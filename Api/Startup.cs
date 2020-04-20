using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Api.Authorization.Handlers;
using Api.Builders;
using Api.Configuration;
using Api.Helpers;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;

namespace Api
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            _configuration = builder.Build();
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, UserRolesHandler>();
            services.AddHstsConfig();
            services.AddResponseCompressionConfig();
            services.AddDatabaseConfig(_configuration);
            services.AddDependencyConfig();
            services.AddIdentityConfig();
            services.AddApiVerisionConfig();
            services.AddAuthenticationConfig(_configuration);
            services.AddAuthorizationConfig();
            services.AddCorsConfig();
            services.AddGlobalizationConfig();
            services.AddControllers();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseApiVersioning();

            app.UseHeadersBuilder();

            /*  List<CultureInfo> cultures = new List<CultureInfo>()
             {
                 new CultureInfo("en-US"),
                 new CultureInfo("pt-BR")
             };

             var options = new RequestLocalizationOptions()
             {
                 DefaultRequestCulture = new RequestCulture("en-US"),
                 SupportedCultures = cultures,
                 SupportedUICultures = cultures
             };

             options.RequestCultureProviders = new []
             {
                 new HeaderDataRequestCultureProvider()
                 {
                     Options = options
                 }
             };

             app.UseRequestLocalization(options); */
            RequestLocalizationOptions options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(options);

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseCors("Default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
