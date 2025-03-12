namespace Aprecia.Bussines.Gateway.Authorization.Storage;

public interface IUnitOfWorkFactory
{
    IUnitOfWorks GetDependencyUnitWork();
}
