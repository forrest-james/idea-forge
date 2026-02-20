using Application.Common.Interfaces;
using System.Security.Cryptography;

namespace WebApp.Services
{
    public sealed class LocalImageStorage : IImageStorage
    {
        private readonly IWebHostEnvironment _environment;
        public LocalImageStorage(IWebHostEnvironment environment) => _environment = environment;

        public async Task<IReadOnlyList<string>> SaveSubmissionImages(Guid submissionId, IReadOnlyList<ImageUpload> files, CancellationToken cancellationToken)
        {
            var results = new List<string>();

            var dir = Path.Combine(_environment.WebRootPath, "images", submissionId.ToString("D"));
            Directory.CreateDirectory(dir);

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName);
                var generated = $"{CreateShortId()}{ext}";
                var physicalPath = Path.Combine(dir, generated);
                await using var stream = File.Create(physicalPath);
                await file.Content.CopyToAsync(stream, cancellationToken);
                results.Add($"/images/{submissionId:D}/{generated}");
            }

            return results;
        }

        private static string CreateShortId()
        {
            Span<byte> bytes = stackalloc byte[8];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}