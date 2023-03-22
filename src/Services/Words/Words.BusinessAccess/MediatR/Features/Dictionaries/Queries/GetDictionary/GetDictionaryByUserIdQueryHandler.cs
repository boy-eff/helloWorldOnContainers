using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos;
using Shared.Exceptions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Queries.GetDictionary;

public class GetDictionaryByUserIdQueryHandler : IRequestHandler<GetDictionaryByUserIdQuery, IEnumerable<WordDto>>
{
    private readonly WordsDbContext _dbContext;

    public GetDictionaryByUserIdQueryHandler(WordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<WordDto>> Handle(GetDictionaryByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(request.UserId);
        
        if (user is null)
        {
            throw new NotFoundException("Dictionary is not found");
        }
        
        var words = await _dbContext.Words
            .Where(w => w.UserDictionaries.Any(uw => uw.UserId == request.UserId))
            .Include(w => w.Translations)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return words.Adapt<IEnumerable<WordDto>>();
    }
}