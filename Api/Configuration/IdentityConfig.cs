using Data.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class IdentityCofig
    {
        public static void AddIdentityConfig(this IServiceCollection services) => IdentitySetup.Add(services);
    }
}