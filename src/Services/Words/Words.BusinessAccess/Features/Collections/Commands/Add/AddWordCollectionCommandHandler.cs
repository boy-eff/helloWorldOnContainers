using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Features.Collections.Commands.Add;

public class AddWordCollectionCommandHandler : IRequestHandler<AddWordCollectionCommand, WordCollectionResponseDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidator<WordCollectionRequestDto> _validator;

    public AddWordCollectionCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor, IValidator<WordCollectionRequestDto> validator)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _validator = validator;
    }

    public async Task<WordCollectionResponseDto> Handle(AddWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var wordCollection = request.WordCollectionCreateDto.Adapt<WordCollection>();
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        wordCollection.UserId = userId!.Value;
        await _dbContext.Collections.AddAsync(wordCollection, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}