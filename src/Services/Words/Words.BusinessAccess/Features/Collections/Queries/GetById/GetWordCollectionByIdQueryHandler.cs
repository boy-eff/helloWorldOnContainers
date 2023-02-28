﻿using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;
using Words.BusinessAccess.Exceptions;
using Words.DataAccess;

namespace Words.BusinessAccess.Features.Collections.Queries.GetById;

public class GetWordCollectionByIdQueryHandler : IRequestHandler<GetWordCollectionByIdQuery, WordCollectionDto>
{
    private readonly WordsDbContext _dbContext;
    private readonly IViewsCounterService _viewsCounterService;

    public GetWordCollectionByIdQueryHandler(WordsDbContext dbContext, IViewsCounterService viewsCounterService)
    {
        _dbContext = dbContext;
        _viewsCounterService = viewsCounterService;
    }

    public async Task<WordCollectionDto> Handle(GetWordCollectionByIdQuery request, CancellationToken cancellationToken)
    {
        var collection = await _dbContext.Collections
            .Include(x => x.Words)
            .ThenInclude(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (collection is null)
        {
            throw new NotFoundException("Collection is not found");
        }
        
        _viewsCounterService.Increment(collection.Id);

        return collection.Adapt<WordCollectionDto>();
    }
}