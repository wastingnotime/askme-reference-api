namespace Askme.Reference.Backend.Api.Configuration;

public interface IMongoDbSettings
{
    string DatabaseName { get; set; }
    string ConnectionString { get; set; }
}