using Askme.Reference.Backend.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Askme.Reference.Backend.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _repository;
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(ILogger<ContactsController> logger, IContactRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<Contact>> Get() =>
        _repository.All();

    [HttpGet("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Contact>> Get(string id)
    {
        var item = await _repository.One(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(Contact value)
    {
        await _repository.Store(value);
        return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
    }

    [HttpPut("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Contact value, string id)
    {
        if (!id.Equals(value.Id))
            return ValidationProblem(); //ValidationProblem instead BadRequest to keep standard 

        var item = await _repository.One(x => x.Id == id);
        if (item is null)
            return NotFound();

        //TODO: repo? responsibility
        item.FirstName = value.FirstName;
        item.LastName = value.LastName;
        item.PhoneNumber = value.PhoneNumber;

        await _repository.Store(item);

        return NoContent();
    }

    [HttpDelete("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id)
    {
        var item = await _repository.One(x => x.Id == id);
        if (item is null)
            return NotFound();

        await _repository.Delete(item);

        return NoContent();
    }
}