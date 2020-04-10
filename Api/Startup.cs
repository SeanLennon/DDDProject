
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using Api.Configuration;
using Api.Controllers;
using Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
// using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /* services.AddMvcCore()
                .AddMvcLocalization()
                .AddDataAnnotationsLocalization()
                .AddDataAnnotations(); */

            services.AddDependencyConfig();
            services.AddIdentityConfig();
            services.AddControllers();
            services.AddDatabaseConfig();
            services.AddApiVerisionConfig();

            services.AddLocalization(x => x.ResourcesPath = "Resources");
            // services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(x =>
            {
                var cultures = new List<CultureInfo>()
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR")
                };
                x.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                x.SupportedCultures = cultures;
                x.SupportedUICultures = cultures;
                x.RequestCultureProviders = new[]
                {
                    new RouteDataRequestCultureProvider
                    {
                        IndexOfCulture = 1,
                        IndexOfUICulture = 1
                    }
                };
                x.SetDefaultCulture("en-US");
            });

            services.Configure<RouteOptions>(x =>
            {
                x.ConstraintMap.Add("Culture", typeof(LanguageRouteConstraint));
            });

            services.AddCors(x =>
            {
                x.AddPolicy("Default", c =>
                {
                    c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = TokenOptions.DefaultProvider;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("de161a0d-984f-4249-9705-5bdc6e02c548")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    RequireExpirationTime = true
                };
            });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("Default", p =>
                {
                    p.RequireRole("User");
                    p.RequireAssertion(c => c.User.IsInRole("User"));
                });
            });

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

            var localize = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localize.Value);

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
