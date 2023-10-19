using System.Linq.Expressions;
using Askme.Reference.Backend.Api.Configuration;
using Askme.Reference.Backend.Api.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Askme.Reference.Backend.Api.Repositories;

public class ContactMongoRepository : IContactRepository
{
    private readonly IMongoCollection<Contact> _contactCollection;

    public ContactMongoRepository(IMongoDbSettings settings) =>
        _contactCollection = new MongoClient(settings.ConnectionString)
            .GetDatabase(settings.DatabaseName)
            .GetCollection<Contact>("contacts");

    public Task<IEnumerable<Contact>> AllAsync() =>
        Task.FromResult(_contactCollection
            .AsQueryable()
            .ToEnumerable());

    public Task<IEnumerable<Contact>> AllAsync(Expression<Func<Contact, bool>> expr) =>
        Task.FromResult(_contactCollection
            .AsQueryable()
            .Where(expr.Compile()));

    public Task<Contact> OneAsync(Expression<Func<Contact, bool>> expr) =>
        _contactCollection
            .AsQueryable()
            .FirstOrDefaultAsync(expr);

    public async Task StoreAsync(Contact contact)
    {
        var filter = Builders<Contact>.Filter.Eq("Id", contact.Id);
        var existingContact = await _contactCollection
            .Find(filter)
            .FirstOrDefaultAsync();

        if (existingContact is null)
            await _contactCollection.InsertOneAsync(contact);
        else
            await _contactCollection.ReplaceOneAsync(filter, contact);
    }

    public Task DeleteAsync(Contact contact) =>
        _contactCollection
            .DeleteOneAsync(Builders<Contact>.Filter.Eq("Id", contact.Id));
}