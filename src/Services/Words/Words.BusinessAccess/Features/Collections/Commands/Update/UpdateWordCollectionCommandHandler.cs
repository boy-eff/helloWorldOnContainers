using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Exceptions;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.Features.Collections.Commands.Update;

public class UpdateWordCollectionCommandHandler : IRequestHandler<UpdateWordCollectionCommand, WordCollectionDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;
    private readonly IValidator<WordCollectionDto> _validator;

    public UpdateWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor,
        WordsDbContext dbContext, IValidator<WordCollectionDto> validator)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<WordCollectionDto> Handle(UpdateWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var existingWordCollection = await _dbContext.Collections
            .FirstOrDefaultAsync(x => x.Id == request.WordCollectionDto.Id && x.UserId == userId,
                cancellationToken: cancellationToken);
        
        if (existingWordCollection is null)
        {
            throw new NotFoundException($"Collection with id {request.WordCollectionDto.Id} is not found for user {userId}");
        }
        
        var wordCollection = request.WordCollectionDto.Adapt(existingWordCollection);
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return wordCollection.Adapt<WordCollectionDto>();
    }
}