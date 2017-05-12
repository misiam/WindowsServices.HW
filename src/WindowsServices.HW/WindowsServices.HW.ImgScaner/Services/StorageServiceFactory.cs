using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServices.HW.ImgScanner.Interfaces;

namespace WindowsServices.HW.ImgScanner.Services
{
    class StorageServiceFactory
    {
        public static IStorageService GetStorageService(string outputLocation)
        {
            return new LocalFolderStorage(outputLocation);
        }
    }
}
