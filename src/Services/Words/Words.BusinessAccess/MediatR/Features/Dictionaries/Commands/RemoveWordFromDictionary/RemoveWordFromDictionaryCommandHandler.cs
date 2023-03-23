using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Shared.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.RemoveWordFromDictionary;

public class RemoveWordFromDictionaryCommandHandler : IRequestHandler<RemoveWordFromDictionaryCommand, int>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RemoveWordFromDictionaryCommandHandler> _logger;

    public RemoveWordFromDictionaryCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<RemoveWordFromDictionaryCommandHandler> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<int> Handle(RemoveWordFromDictionaryCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var wordInDictionary = await _dbContext.UserWords.FindAsync(userId, request.WordId);
        
        if (wordInDictionary is null)
        {
            _logger.LogInformation("Deletion failed: Word with id {WordId} was not found in user {UserId} dictionary", request.WordId, userId);
            throw new NotFoundException("Word is not found in dictionary");
        }

        _dbContext.UserWords.Remove(wordInDictionary);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Word with id {WordId} was successfully removed from user {UserId} dictionary", request.WordId, userId);
        return request.WordId;
    }
}