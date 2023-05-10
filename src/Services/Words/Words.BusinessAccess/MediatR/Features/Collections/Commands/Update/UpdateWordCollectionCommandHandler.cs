using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Shared.Extensions;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.BusinessAccess.Helpers;
using Words.BusinessAccess.Options;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;

public class UpdateWordCollectionCommandHandler : IRequestHandler<UpdateWordCollectionCommand, WordCollectionResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly IOptions<WordsRedisCacheOptions> _wordsRedisCacheOptions;
    private readonly ILogger<UpdateWordCollectionCommandHandler> _logger;
    private readonly ICloudinaryService _cloudinaryService;

    public UpdateWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor,
        WordsDbContext dbContext, 
        IDistributedCache cache, 
        IOptions<WordsRedisCacheOptions> wordsRedisCacheOptions, 
        ILogger<UpdateWordCollectionCommandHandler> logger,
        ICloudinaryService cloudinaryService)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _cache = cache;
        _wordsRedisCacheOptions = wordsRedisCacheOptions;
        _logger = logger;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<WordCollectionResponseDto> Handle(UpdateWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        WordCollection existingWordCollection;
        var cacheKey = CacheHelper.GetCacheKeyForWordCollection(request.Id);
        var isCollectionInCache = _cache.TryGetValue(cacheKey, out existingWordCollection);
        
        if (!isCollectionInCache)
        {
            existingWordCollection = await _dbContext.Collections
                .Include(x => x.Words)
                .FirstOrDefaultAsync(x => x.Id == request.Id,
                    cancellationToken: cancellationToken);
        }
        
        if (existingWordCollection is null)
        {
            _logger.LogInformation("Updating failed: Collection with id {CollectionId} was not found", request.Id);
            throw new NotFoundException($"Collection with id {request.Id} was not found");
        }

        if (existingWordCollection.UserId != userId)
        {
            _logger.LogInformation("Updating failed: User with id {UserId} has no permission to update {CollectionId}", userId, request.Id);
            throw new ForbiddenException($"Cannot update collection with id {existingWordCollection.Id}");
        }
        
        _dbContext.Words.RemoveRange(existingWordCollection.Words);

        var wordCollection = request.WordCollectionDto.Adapt(existingWordCollection);

        if (request.WordCollectionDto.Image is not null)
        {
            var uploadResult = await _cloudinaryService.AddPhotoAsync(request.WordCollectionDto.Image);
            var previousPublicId = wordCollection.ImagePublicId;
            wordCollection.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
            wordCollection.ImagePublicId = uploadResult.PublicId;
        
            if (uploadResult.Error is not null)
            {
                _logger.LogError("Error while uploading image to Cloudinary: {ErrorMessage}", uploadResult.Error.Message);
                throw new InternalServerException("Error while uploading image to external data source");
            }

            await _cloudinaryService.DeletePhotoAsync(previousPublicId);
        }
        
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Word collection with id {CollectionId} was successfully updated", wordCollection.Id);

        if (!isCollectionInCache)
        {
            return wordCollection.Adapt<WordCollectionResponseDto>();
        }

        await wordCollection.AddToCacheAsync(_cache, _wordsRedisCacheOptions);
        
        _logger.LogInformation("Updated word collection with id {CollectionId} was successfully cached after updating", wordCollection.Id);

        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}