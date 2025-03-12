using Aprecia.DataAccess.Gateway.Test.Storge;
using Aprecia.OnBoarding.Gateway.Test.Storage;

namespace Aprecia.DI.Gateway.Test.Helper;

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
