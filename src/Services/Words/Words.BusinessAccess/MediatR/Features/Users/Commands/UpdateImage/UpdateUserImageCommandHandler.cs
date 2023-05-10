using CloudinaryDotNet.Actions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Extensions;
using Words.BusinessAccess.Contracts;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Users.Commands.UpdateImage;

public class UpdateUserImageCommandHandler: IRequestHandler<UpdateUserImageCommand, Unit>
{
    private readonly WordsDbContext _dbContext;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<UpdateUserImageCommandHandler> _logger;

    public UpdateUserImageCommandHandler(WordsDbContext dbContext, ICloudinaryService cloudinaryService, IHttpContextAccessor contextAccessor, ILogger<UpdateUserImageCommandHandler> logger)
    {
        _dbContext = dbContext;
        _cloudinaryService = cloudinaryService;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext.User.GetUserId();

        var user = await _dbContext.Users.FindAsync(userId);

        if (user is null)
        {
            _logger.LogInformation("User with id {UserId} was not found in database", userId);
            throw new NotFoundException("User was not found");
        }
        
        var imageUploadResult = await _cloudinaryService.AddPhotoAsync(request.File);

        if (imageUploadResult.Error is not null)
        {
            _logger.LogError("Error while uploading image to Cloudinary: {ErrorMessage}", imageUploadResult.Error.Message);
            throw new InternalServerException("Error while uploading image to external data source");
        }

        _updateUserImageInformation(imageUploadResult, user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private void _updateUserImageInformation(UploadResult imageUploadResult, User user)
    {
        user.ImageUrl = imageUploadResult.SecureUrl.AbsoluteUri;
        user.ImagePublicId = imageUploadResult.PublicId;
    }
}