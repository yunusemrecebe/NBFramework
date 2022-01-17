using NbFramework.CrossCuttingConcerns.Logging;
using NLog;

namespace NbFramework
{
    public class NBFramework : INBFramework
    {

        static readonly Lazy<INBFramework> _current = new Lazy<INBFramework>(() => new NBFramework());

        public static INBFramework Current
        {
            get
            {
                return _current.Value;
            }
        }

        private NBFramework()
        {

        }

        public DateTime SystemMinDate { get => DateTime.MinValue; }
        public DateTime SystemMaxDate { get => DateTime.MaxValue; }
        public DateTime SystemProcessTime { get => DateTime.UtcNow; }
        public ILoggerService Logger { get => new LoggerServiceBase(); }
    }
}
