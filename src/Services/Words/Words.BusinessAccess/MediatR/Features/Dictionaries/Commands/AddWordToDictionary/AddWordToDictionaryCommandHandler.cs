using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Shared.Messages;
using Shared.Exceptions;
using Shared.Extensions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.AddWordToDictionary;

public class AddWordToDictionaryCommandHandler : IRequestHandler<AddWordToDictionaryCommand, int>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AddWordToDictionaryCommandHandler> _logger;

    public AddWordToDictionaryCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, IPublishEndpoint publishEndpoint, ILogger<AddWordToDictionaryCommandHandler> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<int> Handle(AddWordToDictionaryCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var word = await _dbContext.Words.FindAsync(request.WordId);
        
        if (word is null)
        {
            _logger.LogInformation("Failed to add: Word with id {WordId} was not found", word.Id);
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
        _logger.LogInformation("Word with id {WordId} successfully added to user {UserId} dictionary", word.Id, userId);
        
        return request.WordId;
    }
}