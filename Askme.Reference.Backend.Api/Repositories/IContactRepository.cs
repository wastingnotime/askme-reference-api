using System.Linq.Expressions;
using Askme.Reference.Backend.Api.Models;

namespace Askme.Reference.Backend.Api.Repositories;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> AllAsync();
    Task<IEnumerable<Contact>> AllAsync(Expression<Func<Contact, bool>> expr);
    Task<Contact> OneAsync(Expression<Func<Contact, bool>> expr);
    Task StoreAsync(Contact contact);
    Task DeleteAsync(Contact contact);
}