using NLog;

namespace NbFramework.CrossCuttingConcerns.Logging
{
    public class LoggerServiceBase : ILoggerService
    {
        private Logger Logger;

        public LoggerServiceBase()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public bool IsInfoEnabled => Logger.IsInfoEnabled;
        public bool IsDebugEnabled => Logger.IsDebugEnabled;
        public bool IsWarnEnabled => Logger.IsWarnEnabled;
        public bool IsFatalEnabled => Logger.IsFatalEnabled;
        public bool IsErrorEnabled => Logger.IsErrorEnabled;

        public void Info(object logMessage)
        {
            if (IsInfoEnabled)
                Logger.Info(logMessage);
        }

        public void Debug(object logMessage)
        {
            if (IsDebugEnabled)
                Logger.Debug(logMessage);
        }

        public void Warn(object logMessage)
        {
            if (IsWarnEnabled)
                Logger.Warn(logMessage);
        }

        public void Fatal(object logMessage)
        {
            if (IsFatalEnabled)
                Logger.Fatal(logMessage);
        }

        public void Error(object logMessage)
        {
            if (IsErrorEnabled)
                Logger.Error(logMessage);
        }
    }
}
