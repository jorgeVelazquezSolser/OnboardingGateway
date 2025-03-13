using Aprecia.Bussines.Gateway.People.Storage;
using Aprecia.DataAccess.Gateway.People.Storage;

namespace Aprecia.DI.Gateway.People.Helper;

public class UnitOfWorkFactory: IUnitOfWorkFactory
{
    private readonly string _url;
    public UnitOfWorkFactory(string url)
    {
        _url = url;
    }
    public IUnitOfWorks GetDependencyUnitWork()
    {
        return new UnitOfWork(_url);
    }
}
