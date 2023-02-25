using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.Features.Collections.Commands.Delete;

public class DeleteWordCollectionCommandHandler : IRequestHandler<DeleteWordCollectionCommand, int?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;

    public DeleteWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor, WordsDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<int?> Handle(DeleteWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var wordCollection = await _dbContext.Collections.FirstOrDefaultAsync(x => x.Id == request.WordCollectionId 
            && x.UserId == userId, cancellationToken: cancellationToken);
        
        if (wordCollection is null)
        {
            throw new NotFoundException($"Collection with id {request.WordCollectionId} is not found");
        }

        _dbContext.Collections.Remove(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return request.WordCollectionId;
    }
}