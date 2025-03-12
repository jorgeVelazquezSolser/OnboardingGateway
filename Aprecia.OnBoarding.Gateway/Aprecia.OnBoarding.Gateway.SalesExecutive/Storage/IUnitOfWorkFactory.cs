namespace Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;

public interface IUnitOfWorkFactory
{
    IUnitOfWorks GetDependencyUnitWork();
}
