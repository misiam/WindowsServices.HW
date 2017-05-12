using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using WindowsServices.HW.ImgScanner.Utils;

namespace WindowsServices.HW.ImgScanner.Services
{
    public class ScannerService
    {
        private readonly Scanner _scanner;

        public ScannerService(string inputFolders, string outputFolder, string scanInterval)
        {
            var folders = inputFolders.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            int interval;
            if (!int.TryParse(scanInterval, out interval))
            {
                interval = 5*1000;
            }
            _scanner = new Scanner(folders, outputFolder, interval);

        }


        public void Start()
        {
            _scanner.StartScan();
        }
        public void Stop()
        {
            _scanner.StopScanning();
        }

    }
}
