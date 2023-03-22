using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Exceptions;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;
using Words.DataAccess.Models;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;

public class UpdateWordCollectionCommandHandler : IRequestHandler<UpdateWordCollectionCommand, WordCollectionResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;
    private readonly IValidator<WordCollectionRequestDto> _validator;
    private readonly IDistributedCache _cache;

    public UpdateWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor,
        WordsDbContext dbContext, IValidator<WordCollectionRequestDto> validator, IDistributedCache cache)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _validator = validator;
        _cache = cache;
    }

    public async Task<WordCollectionResponseDto> Handle(UpdateWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();
        WordCollection existingWordCollection;
        var isCollectionInCash = _cache.TryGetWordCollection(request.Id, out existingWordCollection);
        
        if (!isCollectionInCash)
        {
            existingWordCollection = await _dbContext.Collections
                .FirstOrDefaultAsync(x => x.Id == request.Id,
                    cancellationToken: cancellationToken);
        }
        
        if (existingWordCollection is null)
        {
            throw new NotFoundException($"Collection with id {request.Id} is not found");
        }

        if (existingWordCollection.UserId != userId)
        {
            throw new ForbiddenException($"Cannot update collection with id {existingWordCollection.Id}");
        }

        var wordCollection = request.WordCollectionDto.Adapt(existingWordCollection);
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        if (isCollectionInCash)
        {
            await _cache.SetWordCollectionAsync(wordCollection);
        }
        
        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}