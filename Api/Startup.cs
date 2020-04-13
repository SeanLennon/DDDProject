using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Api.Configuration;
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
            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml+json" });
            })
            .Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = false;
            });

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

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });

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
