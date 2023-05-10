using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Options;

namespace Words.BusinessAccess.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private const int PictureWidth = 500;
    private const int PictureHeight = 500;
    
    public CloudinaryService(IOptions<CloudinaryConfigurationOptions> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(PictureHeight).Width(PictureWidth)
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }

    public async Task<DelResResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DelResParams()
        {
            PublicIds = new List<string> { publicId },
            ResourceType = ResourceType.Image
        };

        // Delete the photo from Cloudinary
        var deletionResult = await _cloudinary.DeleteResourcesAsync(deleteParams);

        return deletionResult;
    }
}