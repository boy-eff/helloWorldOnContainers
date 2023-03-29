using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Words.BusinessAccess.Dtos.CollectionRating;
using Shared.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Add;

public class AddRatingCommandHandler : IRequestHandler<AddRatingCommand, CollectionRatingResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AddRatingCommandHandler> _logger;

    public AddRatingCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<AddRatingCommandHandler> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<CollectionRatingResponseDto> Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        var wordCollection = await _dbContext.Collections.FirstOrDefaultAsync(x 
            => x.Id == request.WordCollectionId, cancellationToken: cancellationToken);

        if (wordCollection is null)
        {
            _logger.LogInformation("Failed to add: Word collection with id {CollectionId} was not found", request.WordCollectionId);
            throw new NotFoundException($"Collection with id {request.WordCollectionId} was not found");
        }
        
        
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();

        var existingRating = await _dbContext.WordCollectionRatings.FirstOrDefaultAsync(x => 
            x.UserId == userId && request.WordCollectionId == x.CollectionId, cancellationToken: cancellationToken);

        if (existingRating is not null)
        {
            _logger.LogInformation("Failed to add: Rating from user {UserId} to word collection {CollectionId} already exists",
                userId, existingRating.CollectionId);
            throw new WrongActionException("Rating already exists");
        }

        var rating = new WordCollectionRating()
        {
            CollectionId = request.WordCollectionId, 
            Rating = request.RequestDto.Rating, 
            UserId = userId
        };
        
        await _dbContext.WordCollectionRatings.AddAsync(rating, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Rating from user {UserId} to word collection {CollectionId} was successfully added",
            userId, rating.CollectionId);
        return rating.Adapt<CollectionRatingResponseDto>();
    }
}