using Aprecia.Bussines.Gateway.People.Storage;

namespace Aprecia.Bussines.Gateway.People.Base;

public abstract class RequestHandlerBase
{
    private readonly IUnitOfWorks _unitOfWorks;

    public RequestHandlerBase(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorks = unitOfWorkFactory.GetDependencyUnitWork();
    }

    public IUnitOfWorks UnitOfWorks { get { return _unitOfWorks; } }
}
