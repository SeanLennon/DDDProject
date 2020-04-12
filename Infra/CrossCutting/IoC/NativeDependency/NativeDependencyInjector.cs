using Data.Context;
using Data.Repositories;
using Data.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Identity.Context;
using Identity.Handlers;
using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace IoC.NativeDependency
{
    public static class NativeDependencyInjector
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddTransient<UserDbContext>();
            services.AddTransient<AppDbContext>();
            services.AddScoped<UserManager<User>>();
            services.AddTransient<UserHandler>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<AuthenticatedUser>();
            services.AddScoped<ResetPasswordContextAccessor>();
            services.AddScoped<ProfileUserCommand>();
            services.AddScoped<ChangeNameCommand>();
            services.AddScoped<ChangePasswordCommand>();
            services.AddScoped<ResetPasswordCommand>();
        }
    }
}