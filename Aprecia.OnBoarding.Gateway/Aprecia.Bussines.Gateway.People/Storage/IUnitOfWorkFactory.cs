namespace Aprecia.Bussines.Gateway.People.Storage;

public interface IUnitOfWorkFactory
{
    IUnitOfWorks GetDependencyUnitWork();
}
