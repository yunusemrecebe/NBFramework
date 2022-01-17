namespace NbFramework.CrossCuttingConcerns.Logging
{
    public interface ILoggerService
    {
        void Info(object logMessage);
        void Debug(object logMessage);
        void Warn(object logMessage);
        void Fatal(object logMessage);
        void Error(object logMessage);
    }
}
