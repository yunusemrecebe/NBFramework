using Microsoft.Extensions.DependencyInjection;

namespace NbFramework.Utilities.IoC
{
    public interface ICoreModule
    {
        void LoadModules(IServiceCollection services);
    }
}
