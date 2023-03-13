using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Ratings.Commands.Delete;

public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, int>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteRatingCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var rating = await _dbContext.WordCollectionRatings
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == userId, cancellationToken: cancellationToken);

        if (rating is null)
        {
            throw new NotFoundException("Rating is not found");
        }

        _dbContext.WordCollectionRatings.Remove(rating);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return rating.Id;
    }
}