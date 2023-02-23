using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.Features.Collections.Commands;

public class AddWordCollectionCommandHandler : IRequestHandler<AddWordCollectionCommand, WordCollectionDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddWordCollectionCommandHandler(WordsDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WordCollectionDto> Handle(AddWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var wordCollection = request.WordCollectionCreateDto.Adapt<WordCollection>();
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        
        if (userId is null or 0)
        {
            return null;
        }

        wordCollection.UserId = userId.Value;
        await _dbContext.Collections.AddAsync(wordCollection, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return wordCollection.Adapt<WordCollectionDto>();
    }
}