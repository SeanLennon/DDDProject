using Data.Context;
using Data.Repositories;
using Data.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Identity.Context;
using Identity.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.NativeDependency
{
    public static class NativeDependencyInjector
    {
        public static void Inject(IServiceCollection services)
        {
            services.AddScoped<UserDbContext>();
            services.AddScoped<AppDbContext>();
            services.AddScoped<UserManager<User>>();
            services.AddTransient<UserHandler>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}