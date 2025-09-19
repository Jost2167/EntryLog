namespace EntryLog.Business.ImageBB;

public interface ILoadImagesService
{
    public Task<string> UploadImageAsync(Stream imageStream, string imageName, string extension);
}