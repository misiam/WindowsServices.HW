using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServices.HW.ImgScanner.Interfaces;

namespace WindowsServices.HW.ImgScanner.Services
{
    class LocalFolderStorage : IStorageService
    {
        private string _outputLocation;

        public LocalFolderStorage(string outputLocation)
        {
            this._outputLocation = outputLocation;
        }

        public void SaveToStorage(string fileName)
        {
            System.IO.File.Move(fileName, Path.Combine(_outputLocation, Path.GetFileName(fileName)));
        }
    }
}
