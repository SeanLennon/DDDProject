
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Api.Configuration;
using Api.Extensions;
using Api.Helpers;
using Data.Context;
using Identity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
// using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

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
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseConfig(_configuration);
            services.AddDependencyConfig();
            services.AddIdentityConfig();
            services.AddApiVerisionConfig();
            services.AddAuthorizationConfig();
            services.AddAuthenticationConfig(_configuration);
            services.AddCorsConfig();
            services.AddGlobalizationConfig();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseApiVersioning();

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
