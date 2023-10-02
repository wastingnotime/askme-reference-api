namespace Askme.Reference.Backend.Api.Repositories;

public class ContactRepository : IContactRepository
{
    private static IEnumerable<Contact> _memoryStore = Enumerable.Empty<Contact>();
    private static bool _started;

    public ContactRepository()
    {
        if (!_started)
        {
            _memoryStore = _memoryStore.Append(new Contact
            {
                FirstName = "Albert",
                LastName = "Einsten",
                PhoneNumber = "1111-1111"
            });
            _started = true;
        }
    }

    public Task<IEnumerable<Contact>> All()
    {
        return Task.FromResult(_memoryStore.AsEnumerable());
    }

    public Task<IEnumerable<Contact>> All(Func<Contact, bool> predicate)
    {
        return Task.FromResult(_memoryStore.AsEnumerable().Where(predicate));
    }

    public Task<Contact?> One(Func<Contact, bool> predicate)
    {
        return Task.FromResult(_memoryStore.AsEnumerable().FirstOrDefault(predicate));
    }

    public Task Store(Contact contact)
    {
        if (_memoryStore.AsEnumerable().Any(x => x.Id == contact.Id))
            return Task.CompletedTask;

        _memoryStore = _memoryStore.Append(contact);
        return Task.CompletedTask;
    }

    public Task Delete(Contact contact)
    {
        _memoryStore = _memoryStore.Where(x => x.Id != contact.Id);
        return Task.CompletedTask;
    }
}