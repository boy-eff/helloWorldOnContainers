using Mapster;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Messages;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Add;

public class AddWordCollectionCommandHandler : IRequestHandler<AddWordCollectionCommand, WordCollectionResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AddWordCollectionCommandHandler> _logger;
    private readonly ICloudinaryService _cloudinaryService;

    public AddWordCollectionCommandHandler(WordsDbContext dbContext, 
        IHttpContextAccessor httpContextAccessor, 
        IPublishEndpoint publishEndpoint, 
        ILogger<AddWordCollectionCommandHandler> logger, ICloudinaryService cloudinaryService)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<WordCollectionResponseDto> Handle(AddWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var wordCollection = request.WordCollectionCreateDto.Adapt<WordCollection>();
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        wordCollection.UserId = userId!.Value;

        var uploadResult = await _cloudinaryService.AddPhotoAsync(request.WordCollectionCreateDto.Image);

        wordCollection.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
        wordCollection.ImagePublicId = uploadResult.PublicId;
        
        if (uploadResult.Error is not null)
        {
            _logger.LogError("Error while uploading image to Cloudinary: {ErrorMessage}", uploadResult.Error.Message);
            throw new InternalServerException("Error while uploading image to external data source");
        }
        
        await _dbContext.Collections.AddAsync(wordCollection, cancellationToken);

        var message = new WordCollectionCreatedMessage()
        {
            CreatorId = userId.Value,
            WordCollectionId = wordCollection.Id
        };

        await _publishEndpoint.Publish(message, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Word collection with id {CollectionId} successfully created by user {UserId}", wordCollection.Id, userId);
        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}