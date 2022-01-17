using NbFramework.CrossCuttingConcerns.Logging;

namespace NbFramework
{
    public interface INBFramework
    {
        public DateTime SystemMinDate { get; }
        public DateTime SystemMaxDate { get; }
        public DateTime SystemProcessTime { get; }

        public ILoggerService Logger { get; }
    }
}
