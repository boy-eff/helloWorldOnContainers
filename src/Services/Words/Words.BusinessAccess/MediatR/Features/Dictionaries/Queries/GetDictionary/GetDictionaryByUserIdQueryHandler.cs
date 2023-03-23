using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Words.BusinessAccess.Dtos;
using Shared.Exceptions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Queries.GetDictionary;

public class GetDictionaryByUserIdQueryHandler : IRequestHandler<GetDictionaryByUserIdQuery, IEnumerable<WordDto>>
{
    private readonly WordsDbContext _dbContext;
    private readonly ILogger<GetDictionaryByUserIdQueryHandler> _logger;

    public GetDictionaryByUserIdQueryHandler(WordsDbContext dbContext, ILogger<GetDictionaryByUserIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<WordDto>> Handle(GetDictionaryByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(request.UserId);
        
        if (user is null)
        {
            _logger.LogInformation("Failed to retrieve: Dictionary with id {DictionaryId} was not found", request.UserId);
            throw new NotFoundException($"Dictionary with id {request.UserId} was not found");
        }
        
        var words = await _dbContext.Words
            .Where(w => w.UserDictionaries.Any(uw => uw.UserId == request.UserId))
            .Include(w => w.Translations)
            .ToListAsync(cancellationToken: cancellationToken);
        
        _logger.LogInformation("Dictionary with id {DictionaryId} was successfully retrieved", request.UserId);
        
        return words.Adapt<IEnumerable<WordDto>>();
    }
}