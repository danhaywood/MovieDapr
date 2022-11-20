using ILogger = zipkin4net.ILogger;

namespace MovieFrontend
{
    public class ZipkinClientLogger : ILogger
    {
        public void LogInformation(string message)
        {
            Console.WriteLine($"info: {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"warn: {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"error: {message}");
        }
    }
}