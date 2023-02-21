using Microsoft.AspNetCore.Mvc;
using Words.BusinessAccess.Contracts;
using Words.BusinessAccess.Dtos;

namespace Words.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WordCollectionController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    public WordCollectionController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var collections =  await _collectionService.GetAsync();
        return Ok(collections);
    }

    [HttpPost]
    public async Task<IActionResult> InsertAsync([FromBody] WordCollectionCreateDto wordCollectionCreateDto)
    {
        var id = await _collectionService.InsertAsync(wordCollectionCreateDto);
        return Ok(id);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] WordCollectionDto wordCollectionDto)
    {
        var id = await _collectionService.UpdateAsync(wordCollectionDto);
        return Ok(id);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var deletedId = await _collectionService.DeleteAsync(id);
        if (deletedId == 0)
        {
            return NotFound("Collection is not found");
        }
        return Ok(deletedId);
    }
}