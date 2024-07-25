using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using GPMS.Backend.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly StorageClient _storageClient;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        public FirebaseStorageService
        (IConfiguration configuration,
        EntityListErrorWrapper entityListErrorWrapper,
        StorageClient storageClient)
        {
            _configuration = configuration;
            _entityListErrorWrapper = entityListErrorWrapper;
            _storageClient = storageClient;

        }
        public async Task<string> UploadFile(string filePath, IFormFile file)
        {
            ValidateFilePath(filePath);
            string bucket_Name = _configuration["Firebase:Bucket_Name"];
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var result = await _storageClient
            .UploadObjectAsync(bucket_Name, filePath, file.ContentType, memoryStream);
            return result.MediaLink;
        }
        private void ValidateFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "File Path Is Required");
            }
        }
    }
}