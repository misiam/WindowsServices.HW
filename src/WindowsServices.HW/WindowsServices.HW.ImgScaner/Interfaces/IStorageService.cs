using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServices.HW.ImgScanner.Interfaces
{
    public interface IStorageService
    {
        void SaveToStorage(string fileName);
    }
}
