using Domain.Interfaces.Managers;
using NLog;

namespace Identity.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void Debug(string message) => logger.Debug(message);

        public void Error(string message) => logger.Error(message);

        public void Info(string message) => logger.Info(message);

        public void Warn(string message) => logger.Warn(message);
    }
}