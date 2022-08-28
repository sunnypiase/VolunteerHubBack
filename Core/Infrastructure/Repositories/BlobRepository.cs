using Application.Repositories;
using Application.Repositories.Abstractions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infrastructure.Services;

namespace Infrastructure.Repositories
{
    internal class BlobRepository : IBlobRepository
    {
        public readonly BlobServiceClient _blobServiceClient;

        public BlobRepository(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<bool> DeleteImage(string name)
        {
            BlobContainerClient? containerClient = _blobServiceClient.GetBlobContainerClient("images");
            BlobClient? blobClient = containerClient.GetBlobClient(name);
            return await blobClient.DeleteIfExistsAsync();
        }

        public async Task<IBlobInfo> GetImageByName(string name)
        {
            BlobContainerClient? containerClient = _blobServiceClient.GetBlobContainerClient("images");

            BlobClient? blobClient = containerClient.GetBlobClient(name);

            Azure.Response<BlobDownloadInfo>? blobDownloadInfo = await blobClient.DownloadAsync();

            return new BlobInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task<IBlobInfo> UploadImage(string path, string name)
        {
            BlobContainerClient? containerClient = _blobServiceClient.GetBlobContainerClient("images");

            BlobClient? blobClient = containerClient.GetBlobClient(name);

            await blobClient.UploadAsync(path, new BlobHttpHeaders { ContentType = path.GetContentType() });

            Azure.Response<BlobDownloadInfo>? blobDownloadInfo = await blobClient.DownloadAsync();
            return new BlobInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }
    }
}
