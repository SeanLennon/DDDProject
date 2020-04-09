using IoC.NativeDependency;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class DependencyConfig
    {
        public static void AddDependencyConfig(this IServiceCollection services) => NativeDependencyInjector.Inject(services);
    }
}