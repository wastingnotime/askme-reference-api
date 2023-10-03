using Askme.Reference.Backend.Api.Controllers;
using Askme.Reference.Backend.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Askme.Reference.Backend.Api.Test.Controllers;

public class ContactControllerTest
{
    [Fact]
    public async Task Cannot_get_all_contacts_due_its_nonexistence()
    {
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(x => x.All())
            .ReturnsAsync(Enumerable.Empty<Contact>());

        var actual = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Get();

        Assert.NotNull(actual);
        Assert.Empty(actual);
    }

    [Fact]
    public async Task Can_get_all_contacts()
    {
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(x => x.All())
            .ReturnsAsync(
                new[]
                {
                    new Contact { FirstName = "Albert", LastName = "Einstein", PhoneNumber = "2222-1111" },
                    new Contact { FirstName = "Marie", LastName = "Curie", PhoneNumber = "1111-1111" }
                });

        var actual = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Get();

        Assert.NotNull(actual);
        Assert.Equal(2, actual.Count());
        Assert.Equal("Albert", actual.First().FirstName);
        Assert.Equal("Marie", actual.Skip(1).First().FirstName);
    }

    [Fact]
    public async Task Cannot_get_one_contact_due_its_nonexistence()
    {
        var repositoryMock = new Mock<IContactRepository>();

        var result = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Get(Guid.NewGuid().ToString());
        
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Can_get_one_contact()
    {
        var expected = new Contact
            { Id = Guid.NewGuid().ToString(), FirstName = "Albert", LastName = "Einstein", PhoneNumber = "2222-1111" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(x => x.One(It.IsAny<Func<Contact,bool>>()))
            .ReturnsAsync(expected);

        var result = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Get(expected.Id);
        var actual = ((OkObjectResult) result.Result).Value as Contact; //TODO: review type

        Assert.NotNull(actual);
        Assert.Equal(expected.FirstName, actual.FirstName);
        Assert.Equal(expected.LastName, actual.LastName);
        Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
        
        //todo: verify in parameters on mock object
    }
    
    [Fact]
    public async Task Cannot_delete_a_contact_due_its_nonexistence()
    {
        var sample = new Contact
            { Id = Guid.NewGuid().ToString(), FirstName = "Albert", LastName = "Einstein", PhoneNumber = "2222-1111" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(x => x.Delete(It.IsAny<Contact>()))
            .Returns(Task.CompletedTask);

        var result = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Delete(sample.Id);

        Assert.IsType<NotFoundResult>(result);
    }
 
    [Fact]
    public async Task Can_delete_a_contact()
    {
        var sample = new Contact
            { Id = Guid.NewGuid().ToString(), FirstName = "Albert", LastName = "Einstein", PhoneNumber = "2222-1111" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(x => x.One(It.IsAny<Func<Contact,bool>>()))
            .ReturnsAsync(sample);
        repositoryMock
            .Setup(x => x.Delete(It.IsAny<Contact>()))
            .Returns(Task.CompletedTask);

        var result = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Delete(sample.Id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Can_create_a_contact()
    {
        var expected = new Contact
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Albert",
            LastName = "Einstein",
            PhoneNumber = "2222-1111"
        };

        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(x => x.Store(expected))
            .Returns(Task.CompletedTask);

        var result = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Post(expected);

        Assert.IsType<CreatedAtActionResult>(result);
        //todo: verify id
        var actual = ((CreatedAtActionResult)result).Value as Contact;
        Assert.Equal(expected.FirstName, actual.FirstName);
        Assert.Equal(expected.LastName, actual.LastName);
        Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
    }

    [Fact]
    public async Task Can_update_a_contact()
    {
        var current = new Contact
            { Id = Guid.NewGuid().ToString(), FirstName = "Albert", LastName = "Einstein", PhoneNumber = "2222-1111" };
        var repositoryMock = new Mock<IContactRepository>();
        repositoryMock
            .Setup(x => x.One(It.IsAny<Func<Contact,bool>>()))
            .ReturnsAsync(current);
    
        var changed =    current.Clone();
        changed.FirstName = "Ulbert";
        changed.LastName = "Oinstein";
        changed.PhoneNumber = "3333-4444";
    
        var result = await new ContactsController(GetLoggerStub(), repositoryMock.Object).Update(changed, changed.Id);
        
        Assert.IsType<NoContentResult>(result);

        repositoryMock.Verify( x=>x.One(It.IsAny<Func<Contact,bool>>()), Times.AtMostOnce());
        repositoryMock.Verify(
            x => x.Store(
                It.Is<Contact>(c =>
                c.Id == changed.Id &&
                c.FirstName == changed.FirstName &&
                c.LastName == changed.LastName &&
                c.PhoneNumber == changed.PhoneNumber))
            , Times.AtMostOnce());
        repositoryMock.VerifyNoOtherCalls();
    }
   
    private static ILogger<ContactsController> GetLoggerStub() => new Mock<ILogger<ContactsController>>().Object;

}