using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GPMS.Backend.Services.Services
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFile(string filePath, IFormFile file);
    }
}