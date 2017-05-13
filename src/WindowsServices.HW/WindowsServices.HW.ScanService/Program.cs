using System;
using System.Configuration;
using System.IO;
using System.Linq;
using WindowsServices.HW.ImgScanner.Services;
using Topshelf;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace WindowsServices.HW.ScanService
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFolders = string.Empty;
            string outputFolder = string.Empty;
            string scanInterval = string.Empty;

            string logPath = GetLogPath(args); 


            var logConfig = new LoggingConfiguration();
            var target = new FileTarget()
            {
                Name = "Def",
                FileName = logPath,
                Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
            };

            logConfig.AddTarget(target);
            logConfig.AddRuleForAllLevels(target);

            var logFactory = new LogFactory(logConfig);

            var logger = ImgScanner.Utils.Logger.Current;
            logger.SetActualLogger(logFactory.GetLogger("Topshelf"));

            HostFactory.Run(x =>
            {
                x.AddCommandLineDefinition("inputFolders", i => { inputFolders = i; logger.LogInfo(i); });
                x.AddCommandLineDefinition("outputFolder", o => { outputFolder = o; logger.LogInfo(o); });
                x.AddCommandLineDefinition("scanInterval", si => { scanInterval = si; logger.LogInfo(si); });
                x.AddCommandLineDefinition("logPath", log => {  });

                x.Service<ScannerService>(
                    
                    conf=>
                    {
                        conf.ConstructUsing(() => new ScannerService(inputFolders, outputFolder, scanInterval));
                        conf.WhenStarted((service, host) => {  service.Start() ;return true; });
                        conf.WhenStopped(service => service.Stop());
                    }).UseNLog(logFactory);

                x.SetDescription("Scaner Service");
                x.SetDisplayName("Scaner Service");
                x.SetServiceName("WindowsServices.HW.ScanService");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
            });
        }

        private static string GetLogPath(string[] args)
        {
            const string LOG = "-logPath:";
            string arg = args.FirstOrDefault(x => x.StartsWith(LOG));
            if (arg == null )
            {
                return @"C:\winserv\scanner.log";
            }
            
            return new string(arg.Skip(LOG.Length).ToArray());
        }
    }
}
