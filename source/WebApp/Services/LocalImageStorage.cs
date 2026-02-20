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

        public Task DeleteByAbsoluteUrl(string absoluteUrl, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(absoluteUrl))
                return Task.CompletedTask;

            if (!Uri.TryCreate(absoluteUrl, UriKind.Absolute, out var uri))
                return Task.CompletedTask;

            var path = uri.AbsolutePath;
            if (string.IsNullOrWhiteSpace(path))
                return Task.CompletedTask;

            if (!path.StartsWith("/images/", StringComparison.OrdinalIgnoreCase))
                return Task.CompletedTask;

            var relative = path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(_environment.WebRootPath, relative);

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            return Task.CompletedTask;
        }

        private static string CreateShortId()
        {
            Span<byte> bytes = stackalloc byte[8];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}