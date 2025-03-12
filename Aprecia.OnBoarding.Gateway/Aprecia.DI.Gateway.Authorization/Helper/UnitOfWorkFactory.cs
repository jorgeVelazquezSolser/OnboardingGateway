using Aprecia.Bussines.Gateway.Authorization.Storage;
using Aprecia.DataAccess.Gateway.Authorization.Storage;

namespace Aprecia.DI.Gateway.Authorization.Helper;

public class UnitOfWorkFactory: IUnitOfWorkFactory
{
    private readonly string _connectionstring;

    public UnitOfWorkFactory(string connectionstring)
    {
        _connectionstring = connectionstring;
    }
    public IUnitOfWorks GetDependencyUnitWork()
    {
        return new UnitOfWork(_connectionstring);
    }
}
