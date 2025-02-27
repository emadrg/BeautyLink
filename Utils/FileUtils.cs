using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class FileUtils
    {
        public const string BasePath = "E:\\facultate\\Beauty Link\\Schelet\\FileStorage\\";

        public static void CreateFile(IFormFile formFile)
        {
            
            var fullPath = Path.Combine(BasePath, formFile.FileName);
            var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            formFile.CopyTo(fileStream);
        }
    }
}
