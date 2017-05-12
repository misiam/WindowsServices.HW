using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
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
            string outputPath = string.Empty;

            string logName = Path.Combine(@"C:\winserv\", "CustomCommandService.log");

            var logConfig = new LoggingConfiguration();
            var target = new FileTarget()
            {
                Name = "Def",
                FileName = Path.Combine(logName, "log.txt"),
                Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
            };

            logConfig.AddTarget(target);
            logConfig.AddRuleForAllLevels(target);

            var logFactory = new LogFactory(logConfig);


            try
            {

                HostFactory.Run(x =>
                {
                    x.AddCommandLineDefinition("inputFolders", i => { inputFolders = i; });
                    x.AddCommandLineDefinition("outputPath", o => { outputPath = o; });

                    x.Service<ScannerService>(
                    
                        conf=>
                        {
                            conf.ConstructUsing(() => new ScannerService(inputFolders, outputPath));
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
            catch (Exception e)
            {
                File.AppendAllText(logName, string.Format("{0} Command {1} \n", DateTime.Now.ToLongTimeString(), e.ToString()));
                throw;
            }
            //Console.ReadKey();


        }
    }
}
