using System;


namespace WindowsServices.HW.ImgScanner.Interfaces
{
    public interface ILogger
    {
        void LogInfo(string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogError(Exception exception);
        void SetActualLogger(object logger);
    }
}
