namespace Domain.Interfaces.Managers
{
    public interface ILoggerManager
    {
        void LogInfo(string messge);
        void LogWarn(string messge);
        void LogDebug(string messge);
        void LogError(string messge);
    }
}