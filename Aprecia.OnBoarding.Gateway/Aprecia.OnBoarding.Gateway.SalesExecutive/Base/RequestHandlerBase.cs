using Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;

namespace Aprecia.OnBoarding.Gateway.SalesExecutive.Base;

public abstract class RequestHandlerBase
{
    private readonly IUnitOfWorks _unitOfWorks;

    public RequestHandlerBase(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorks = unitOfWorkFactory.GetDependencyUnitWork();
    }

    public IUnitOfWorks UnitOfWorks { get { return _unitOfWorks; } }
}
