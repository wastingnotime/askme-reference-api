using Microsoft.AspNetCore.Mvc;

namespace Askme.Reference.Backend.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ILogger<ContactsController> _logger;
    private static IEnumerable<Contact> _memoryStore = Enumerable.Empty<Contact>();

    public ContactsController(ILogger<ContactsController> logger)
    {
        _logger = logger;
        if (!_memoryStore.Any())
            _memoryStore = _memoryStore.Append(new Contact
            {
                FirstName = "Albert",
                LastName = "Einsten",
                PhoneNumber = "1111-1111"
            });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IEnumerable<Contact>> Get() =>
        Task.FromResult(_memoryStore.AsEnumerable());

    [HttpGet("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<ActionResult<Contact>> Get(string id)
    {
        var item = GetItem(id);
        return Task.FromResult((ActionResult<Contact>)(item is null ? NotFound() : Ok(item)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Task<IActionResult> Post(Contact value)
    {
        _memoryStore = _memoryStore.Append(value);
        return Task.FromResult(CreatedAtAction(nameof(Get), new { id = value.Id }, value) as IActionResult);
    }

    [HttpPut("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> Update(Contact value, string id)
    {
        if (!id.Equals(value.Id))
            return Task.FromResult(ValidationProblem() as IActionResult); //ValidationProblem instead BadRequest to keep standard 

        var item = GetItem(id);
        if (item is null)
            return Task.FromResult(NotFound() as IActionResult);

        item.FirstName = value.FirstName;
        item.LastName = value.LastName;
        item.PhoneNumber = value.PhoneNumber;

        return Task.FromResult(NoContent() as IActionResult);
    }

    [HttpDelete("{id:length(36)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> Delete(string id)
    {
        var item = GetItem(id);
        if (item is null)
            return Task.FromResult(NotFound() as IActionResult);

        _memoryStore = _memoryStore.Where(x => x.Id != item.Id);

        return Task.FromResult(NoContent() as IActionResult);
    }

    private static Contact? GetItem(string id) => 
        _memoryStore.FirstOrDefault(x => x.Id == id);
}