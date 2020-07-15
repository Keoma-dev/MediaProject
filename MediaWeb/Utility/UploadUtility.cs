using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Utility
{
     public class UploadUtility
    {
        

        public static string UploadFile(IFormFile file, string location, IWebHostEnvironment hostEnvironment)
        {            

            if (file != null)
            {
               
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var pathName = Path.Combine(hostEnvironment.WebRootPath, location);
                var fileNameWithPath = Path.Combine(pathName, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return "/"+location+"/" + uniqueFileName;
            }
            else
            {
                return "";
            }
        }
        public void DeleteFile(string webRootPath, string fileUrl)
        {
            if (fileUrl.StartsWith("/"))
            {
                fileUrl = fileUrl.Substring(1);
            }

            string pathName = Path.Combine(webRootPath, fileUrl);
            System.IO.File.Delete(pathName);
        }
       
    }
}
