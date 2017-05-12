using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using WindowsServices.HW.ImgScanner.Interfaces;


namespace WindowsServices.HW.ImgScanner.Services
{
    public class Scanner
    {
        private readonly string[] _inputFolders;
        private readonly string _outputFolder;

        Thread workingThread;
        ManualResetEvent workStop;
        AutoResetEvent newFile;

        private IStorageService _storageService;


        public Scanner(string[] inputFolders, string outputFolder)
        {
            _inputFolders = inputFolders;
            _outputFolder = outputFolder;
            _storageService = StorageServiceFactory.GetStorageService(outputFolder);

            CreateDirectoryIfNotExists(_inputFolders);
            CreateDirectoryIfNotExists(_outputFolder);

            workStop = new ManualResetEvent(false);
            newFile = new AutoResetEvent(false);
            workingThread = new Thread(Scanning);


        }

        private void CreateDirectoryIfNotExists(params string[] folders)
        {
            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
        }


        private void Scanning()
        {
            do
            {
                foreach (var inputFolder in _inputFolders)
                {
                    ScanFolder(inputFolder);
                }

            }
            while (WaitHandle.WaitAny(new WaitHandle[] { workStop, newFile } , 5*1000)!= 0);
        }

        private void ScanFolder(string inputFolder)
        {
            foreach (var file in Directory.EnumerateFiles(inputFolder))
            {
                if (workStop.WaitOne(TimeSpan.Zero))
                    return;

                if (TryOpen(file, 3))
                    _storageService.SaveToStorage(file);
            }
        }

        private bool TryOpen(string fullPath, int v)
        {
            for (int i = 0; i < v; i++)
            {

                try
                {
                    var file = System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.None);
                    file.Close();

                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(3000);
                }
            }

            return false;
        }
        public void StartScan()
        {
            workingThread.Start();

        }

        public void StopScanning()
        {
            workingThread.Abort();
        }
    }
}
