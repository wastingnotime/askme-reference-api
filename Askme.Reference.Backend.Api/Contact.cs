namespace Askme.Reference.Backend.Api;

public class Contact
{
    //string instead guid due its extensible compatibility
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    
    public Contact Clone() =>
        (Contact)MemberwiseClone();
}