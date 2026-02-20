namespace Application.Common.Interfaces
{
    public interface IImageStorage
    {
        Task<IReadOnlyList<string>> SaveSubmissionImages(Guid submissionId, IReadOnlyList<ImageUpload> files, CancellationToken cancellationToken);
        Task DeleteByAbsoluteUrl(string absoluteUrl, CancellationToken cancellationToken);
    }

    public sealed record ImageUpload(string FileName, Stream Content);
}