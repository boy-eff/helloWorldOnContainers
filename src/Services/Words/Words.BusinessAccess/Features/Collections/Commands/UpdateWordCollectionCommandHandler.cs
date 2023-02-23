using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.Features.Collections.Commands;

public class UpdateWordCollectionCommandHandler : IRequestHandler<UpdateWordCollectionCommand, WordCollectionDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;

    public UpdateWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor, WordsDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<WordCollectionDto> Handle(UpdateWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        
        if (userId is null)
        {
            return null;
        }
        
        var existingWordCollection = await _dbContext.Collections
            .FirstOrDefaultAsync(x => x.Id == request.WordCollectionDto.Id && x.UserId == userId, cancellationToken: cancellationToken);
        
        if (existingWordCollection is null)
        {
            return null;
        }
        
        var wordCollection = request.WordCollectionDto.Adapt(existingWordCollection);
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return wordCollection.Adapt<WordCollectionDto>();
    }
}