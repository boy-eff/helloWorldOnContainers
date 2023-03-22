using FluentValidation;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Messages;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Add;

public class AddWordCollectionCommandHandler : IRequestHandler<AddWordCollectionCommand, WordCollectionResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidator<WordCollectionRequestDto> _validator;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddWordCollectionCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, IValidator<WordCollectionRequestDto> validator, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _validator = validator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<WordCollectionResponseDto> Handle(AddWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var wordCollection = request.WordCollectionCreateDto.Adapt<WordCollection>();
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        wordCollection.UserId = userId!.Value;
        await _dbContext.Collections.AddAsync(wordCollection, cancellationToken);

        var message = new WordCollectionCreatedMessage()
        {
            CreatorId = userId.Value,
            WordCollectionId = wordCollection.Id
        };

        await _publishEndpoint.Publish(message, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}