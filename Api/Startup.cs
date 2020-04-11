
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Api.Configuration;
using Api.Extensions;
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
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DatabaseSettings database = Configuration.GetSection("AppConnectionString").Get<DatabaseSettings>();
            var conn = new NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("AppConnectionString"))
            {
                Database = Configuration["DatabaseSettings:Name"],
                Username = Configuration["DatabaseSettings:Username"],
                Password = Configuration["DatabaseSettings:Password"]
            }.ConnectionString;

            services.AddDbContext<UserDbContext>(x =>
                x.UseNpgsql(conn, x =>
                    x.MigrationsAssembly("Api").SetPostgresVersion(0, 1)));

            services.AddDbContext<AppDbContext>(x =>
                x.UseNpgsql(conn, x =>
                    x.MigrationsAssembly("Api").SetPostgresVersion(0, 1)));

            services.AddDependencyConfig();
            services.AddIdentityConfig();
            services.AddControllers();
            // services.AddDatabaseConfig();
            services.AddApiVerisionConfig();
            // services.AddLocalizationConfig();

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

            services.AddLocalization(x => x.ResourcesPath = "Resources");
            services.Configure<RouteOptions>(x =>
            {
                x.ConstraintMap.Add("Content-Language", typeof(LanguageRouteConstraint));
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

            List<CultureInfo> cultures = new List<CultureInfo>()
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

            app.UseRequestLocalization(options);

            // app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

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
