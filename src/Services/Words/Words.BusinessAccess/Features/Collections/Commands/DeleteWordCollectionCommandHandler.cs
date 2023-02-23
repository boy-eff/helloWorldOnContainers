using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.Features.Collections.Commands;

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

        if (userId is null or 0)
        {
            return null;
        }
        var wordCollection = await _dbContext.Collections.FirstOrDefaultAsync(x => x.Id == request.WordCollectionId 
            && x.UserId == userId, cancellationToken: cancellationToken);
        
        if (wordCollection is null)
        {
            return 0;
        }

        _dbContext.Collections.Remove(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return request.WordCollectionId;
    }
}