using CloudinaryDotNet.Actions;

namespace HumanShop.Services.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string  photoId);
    }
}
