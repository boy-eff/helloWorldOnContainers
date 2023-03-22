using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Words.BusinessAccess.Dtos.WordCollection;
using Words.BusinessAccess.Extensions;
using Words.DataAccess;

namespace Words.BusinessAccess.MediatR.Features.Collections.Commands.Update;

public class UpdateWordCollectionCommandHandler : IRequestHandler<UpdateWordCollectionCommand, WordCollectionResponseDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly WordsDbContext _dbContext;
    private readonly IValidator<WordCollectionRequestDto> _validator;

    public UpdateWordCollectionCommandHandler(IHttpContextAccessor httpContextAccessor,
        WordsDbContext dbContext, IValidator<WordCollectionRequestDto> validator)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<WordCollectionResponseDto> Handle(UpdateWordCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor?.HttpContext?.User.GetUserId();

        var existingWordCollection = await _dbContext.Collections
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == userId,
                cancellationToken: cancellationToken);
        
        if (existingWordCollection is null)
        {
            throw new NotFoundException($"Collection with id {request.Id} is not found for user {userId}");
        }
        
        var wordCollection = request.WordCollectionDto.Adapt(existingWordCollection);
        _dbContext.Collections.Update(wordCollection);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return wordCollection.Adapt<WordCollectionResponseDto>();
    }
}