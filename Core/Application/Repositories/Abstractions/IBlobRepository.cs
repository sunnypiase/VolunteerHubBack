namespace Application.Repositories.Abstractions
{
    public interface IBlobRepository
    {
        Task<IBlobInfo> GetImageByName(string name);
        Task<IBlobInfo> UploadImage(string path, string name);
        Task<bool> DeleteImage(string name);

    }
}
