using System.Linq.Expressions;
using Askme.Reference.Backend.Api.Models;
using Askme.Reference.Backend.Api.Repositories;

namespace Askme.Reference.Backend.Api.Test.Repositories;

public class ContactMemoryRepository : IContactRepository
{
    private static IEnumerable<Contact> _memoryStore = Enumerable.Empty<Contact>();
    private static bool _started;

    public ContactMemoryRepository()
    {
        if (_started) return;
        _memoryStore = _memoryStore.Append(new Contact
        {
            FirstName = "Albert",
            LastName = "Einsten",
            PhoneNumber = "1111-1111"
        });
        _started = true;
    }

    public Task<IEnumerable<Contact>> AllAsync() => 
        Task.FromResult(_memoryStore.AsEnumerable());

    public Task<IEnumerable<Contact>> AllAsync(Expression<Func<Contact, bool>> expr) => 
        Task.FromResult(_memoryStore.AsEnumerable().Where(expr.Compile()));

    public Task<Contact> OneAsync(Expression<Func<Contact, bool>> expression) => 
        Task.FromResult(_memoryStore.AsEnumerable().FirstOrDefault(expression.Compile()))!;

    public Task StoreAsync(Contact contact)
    {
        if (_memoryStore.AsEnumerable().Any(x => x.Id == contact.Id))
            return Task.CompletedTask;

        _memoryStore = _memoryStore.Append(contact);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Contact contact)
    {
        _memoryStore = _memoryStore.Where(x => x.Id != contact.Id);
        return Task.CompletedTask;
    }
}