using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TalentFlow.Application.Interfaces;

namespace TalentFlow.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly string _baseUrl;

        public LocalFileStorageService(IConfiguration configuration)
        {
            // هيتخزن جوه wwwroot/uploads
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            _baseUrl = configuration["AppUrlSettings:ApiBaseUrl"] ?? "https://localhost:5001";
        }

        public async Task<FileUploadResult> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
        {
            var folderPath = Path.Combine(_basePath, folder);
            Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(file.FileName);
            var storedFileName = $"{Guid.NewGuid()}{extension}"; // اسم عشوائي، منع أي path traversal
            var fullPath = Path.Combine(folderPath, storedFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            return new FileUploadResult
            {
                Url = $"{_baseUrl}/uploads/{folder}/{storedFileName}",
                FileName = file.FileName
            };
        }
    }
}