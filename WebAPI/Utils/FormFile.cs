using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Utils
{
    public static class FormFileExtension
    {
        public static async Task<Stream> GetStream(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return (Stream) memoryStream;
            }
        }
    }
}
