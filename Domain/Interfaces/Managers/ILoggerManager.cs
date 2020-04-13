namespace Domain.Interfaces.Managers
{
    public interface ILoggerManager
    {
        void Info(string messge);
        void Warn(string messge);
        void Debug(string messge);
        void Error(string messge);
    }
}