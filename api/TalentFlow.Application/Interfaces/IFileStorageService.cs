using Microsoft.AspNetCore.Http;

namespace TalentFlow.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<FileUploadResult> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
    }

    public class FileUploadResult
    {
        public string Url { get; set; } = default!;
        public string FileName { get; set; } = default!;
    }
}