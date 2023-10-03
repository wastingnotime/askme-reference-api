namespace Askme.Reference.Backend.Api.Repositories;

public interface IContactRepository
{
    public Task<IEnumerable<Contact>> AllAsync();
    public Task<IEnumerable<Contact>> AllAsync(Func<Contact, bool> predicate);
    public Task<Contact?> OneAsync(Func<Contact, bool> predicate);
    public Task StoreAsync(Contact contact);
    public Task DeleteAsync(Contact contact);
}