namespace Askme.Reference.Backend.Api.Repositories;

public interface IContactRepository
{
    public Task<IEnumerable<Contact>> All();
    public Task<IEnumerable<Contact>> All(Func<Contact, bool> predicate);
    public Task<Contact?> One(Func<Contact, bool> predicate);
    public Task Store(Contact contact);
    public Task Delete(Contact contact);
}