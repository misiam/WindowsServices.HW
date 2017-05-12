using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace WindowsServices.HW.ImgScanner.Services
{
    public class ScannerService
    {
        private readonly Scanner _scanner;

        public ScannerService(string inputFolders, string outputFolder)
        {
            var folders = inputFolders.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            _scanner = new Scanner(folders, outputFolder);
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
