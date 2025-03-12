using Aprecia.DataAccess.Gateway.SalesExecutive.Storge;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;

namespace Aprecia.DI.Gateway.SalesExecutive.Helper;

public class UnitOfWorkFactory : IUnitOfWorkFactory
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
