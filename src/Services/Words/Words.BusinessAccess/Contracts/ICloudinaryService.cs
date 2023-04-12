using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Words.BusinessAccess.Contracts;

public interface ICloudinaryService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
}