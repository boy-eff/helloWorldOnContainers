using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Features.Collections.Commands;

public class AddWordCollectionCommandHandler : IRequestHandler<AddWordCollectionCommand, WordCollectionDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidator<WordCollectionCreateDto> _validator;

    public AddWordCollectionCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, IValidator<WordCollectionCreateDto> validator)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _validator = validator;
    }

    public async Task<WordCollectionDto> Handle(AddWordCollectionCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request.WordCollectionCreateDto, cancellationToken: cancellationToken);
        var wordCollection = request.WordCollectionCreateDto.Adapt<WordCollection>();
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        
        if (userId is null or 0)
        {
            throw new AuthorizationException();
        }

        wordCollection.UserId = userId.Value;
        await _dbContext.Collections.AddAsync(wordCollection, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return wordCollection.Adapt<WordCollectionDto>();
    }
}