using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Messages;
using Words.BusinessAccess.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.AddWordToDictionary;

public class AddWordToDictionaryCommandHandler : IRequestHandler<AddWordToDictionaryCommand, int>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddWordToDictionaryCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<int> Handle(AddWordToDictionaryCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var word = await _dbContext.Words.FindAsync(request.WordId);
        
        if (word is null)
        {
            throw new NotFoundException("Word is not found");
        }
        
        var userWord = new UserWord()
        {
            UserId = userId,
            WordId = request.WordId
        };
        await _dbContext.UserWords.AddAsync(userWord, cancellationToken);

        var message = new WordAddedToDictionaryMessage()
        {
            DictionaryOwnerId = userId,
            WordId = request.WordId
        };

        await _publishEndpoint.Publish(message, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return request.WordId;
    }
}