using Microsoft.Extensions.DependencyInjection;
using NbFramework.CrossCuttingConcerns;
using NbFramework.CrossCuttingConcerns.Microsoft;
using NbFramework.Utilities.IoC;

namespace NbFramework.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void LoadModules(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();
        }
    }
}
