using Microsoft.Extensions.DependencyInjection;
using NbFramework.Utilities.IoC;

namespace NbFramework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, ICoreModule[] coreModules)
        {
            foreach (ICoreModule coreModule in coreModules)
            {
                coreModule.LoadModules(services);
            }

            return ServiceTool.Create(services);
        }
    }
}
