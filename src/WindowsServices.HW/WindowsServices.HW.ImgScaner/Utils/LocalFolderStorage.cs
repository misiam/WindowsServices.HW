using System.IO;
using WindowsServices.HW.ImgScanner.Interfaces;

namespace WindowsServices.HW.ImgScanner.Utils
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
