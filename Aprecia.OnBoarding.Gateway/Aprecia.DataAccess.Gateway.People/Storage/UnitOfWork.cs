using Aprecia.Bussines.Gateway.People.Storage;

namespace Aprecia.DataAccess.Gateway.People.Storage;

public class UnitOfWork : IUnitOfWorks
{
    public readonly string _url;
    public IPeopleStorage PeopleStorage { get; set; }

    public UnitOfWork(string _url) 
    {
        PeopleStorage = new PeopleStorage(_url);
    }
    
}
