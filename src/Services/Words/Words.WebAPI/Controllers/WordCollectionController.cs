﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;

namespace Words.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WordCollectionController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    public WordCollectionController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    /// <summary>
    /// Get all collections
    /// </summary>
    /// <response code="200">Returns all collections</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<WordCollectionDto>>> GetAsync()
    {
        var wordCollections =  await _collectionService.GetAsync();
        return Ok(wordCollections);
    }

    /// <summary>
    /// Add new collection
    /// </summary>
    /// <remarks>
    /// Sample value of message
    /// 
    ///     POST /api/wordcollection
    ///     {
    ///         "name": "collectionName",
    ///         "englishLevel": 1,
    ///         "words": [
    ///             {
    ///                 "value": "word value in Russian",
    ///                 "translations": [
    ///                     {
    ///                         "translation": "all possible word translations in English"
    ///                     }
    ///                 ]
    ///             }
    ///         ]
    ///     }
    ///     
    /// </remarks>
    /// <response code="200">Returns the newly created collection</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> InsertAsync([FromBody] WordCollectionCreateDto wordCollectionCreateDto)
    {
        var wordCollectionId = await _collectionService.InsertAsync(wordCollectionCreateDto);
        return Ok(wordCollectionId);
    }

    /// <summary>
    /// Update collection
    /// </summary>
    /// <response code="200">Returns updated collection id</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> UpdateAsync([FromBody] WordCollectionDto wordCollectionDto)
    {
        var updatedWordCollectionId = await _collectionService.UpdateAsync(wordCollectionDto);
        return Ok(updatedWordCollectionId);
    }

    /// <summary>
    /// Delete collection
    /// </summary>
    /// <response code="200">Returns deleted collection id</response>
    /// <response code="404">If collection is not found</response>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteAsync(int id)
    {
        var deletedWordCollectionId = await _collectionService.DeleteAsync(id);
        if (deletedWordCollectionId == 0)
        {
            return NotFound("Collection is not found");
        }
        return Ok(deletedWordCollectionId);
    }
}