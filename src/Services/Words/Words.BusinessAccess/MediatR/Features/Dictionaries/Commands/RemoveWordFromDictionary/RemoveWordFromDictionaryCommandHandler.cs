using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.RemoveWordFromDictionary;

public class RemoveWordFromDictionaryCommandHandler : IRequestHandler<RemoveWordFromDictionaryCommand, int>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RemoveWordFromDictionaryCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(RemoveWordFromDictionaryCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var wordInDictionary = await _dbContext.UserWords.FindAsync(userId, request.WordId);
        
        if (wordInDictionary is null)
        {
            throw new NotFoundException("Word is not found in dictionary");
        }

        _dbContext.UserWords.Remove(wordInDictionary);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return request.WordId;
    }
}