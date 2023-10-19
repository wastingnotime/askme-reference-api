using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Askme.Reference.Backend.Api.Models;

public class Contact
{
    //string instead guid/objectId due its extensible compatibility
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    
    public Contact Clone() =>
        (Contact)MemberwiseClone();
}


