using Aprecia.Bussines.Gateway.Authorization.Storage;

namespace Aprecia.Bussines.Gateway.Authorization.Base;

public class RequestHandlerBase
{
    private readonly IUnitOfWorks _unitOfWorks;

    public RequestHandlerBase(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorks = unitOfWorkFactory.GetDependencyUnitWork();
    }

    public IUnitOfWorks UnitOfWorks { get { return _unitOfWorks; } }
}
