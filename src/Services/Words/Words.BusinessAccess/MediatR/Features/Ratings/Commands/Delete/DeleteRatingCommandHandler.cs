using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Delete;

public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, int>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<DeleteRatingCommandHandler> _logger;

    public DeleteRatingCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<DeleteRatingCommandHandler> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<int> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var rating = await _dbContext.WordCollectionRatings
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (rating is null)
        {
            _logger.LogInformation("Deletion failed: Rating {RatingId} was not found", request.Id);
            throw new NotFoundException("Rating is not found");
        }

        if (rating.UserId != userId)
        {
            _logger.LogInformation("Deletion failed: User with id {UserId} has no permission to delete rating {RatingId}", 
                userId, rating.Id);
            throw new ForbiddenException($"Cannot delete rating");
        }

        _dbContext.WordCollectionRatings.Remove(rating);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Word collection rating with id {RatingId} was successfully deleted", rating.Id);
        return rating.Id;
    }
}