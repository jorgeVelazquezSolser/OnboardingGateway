using Aprecia.Domain.Gateway.SalesExecutive.Dtos;
using Aprecia.Domain.Gateway.SalesExecutive.Media;
using Aprecia.Domain.Gateway.Shared.Dtos;
using Aprecia.Domain.Gateway.Shared.Media;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Base;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;
using MediatR;


namespace Aprecia.OnBoarding.Gateway.SalesExecutive.Query;

public class GetSalesExecutiveQueryHandler: RequestHandlerBase, IRequestHandler<GetSalesExecutiveQuery, SalesExecutiveResourceListImpl>
{
    public GetSalesExecutiveQueryHandler(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }

    public async Task<SalesExecutiveResourceListImpl> Handle(GetSalesExecutiveQuery request, CancellationToken cancellationToken)
    {        

        try
        {
            var result = await UnitOfWorks.SalesExecutiveStorage.GetSalesExecutives(cancellationToken);
            var success = new SalesExecutiveResourceListImpl(result);
            success.StatusService = new StatusServiceDto();
            success.StatusService.Status = true;
            return success;
        }
        catch (Exception ex) 
        {
            return DataErrorGenericList.Error<SalesExecutiveResourceListImpl, GetSalesExecutiveResponseDto>(
            ex,
                "Error vuelva a intentar de favor",
                "DA01",
                1,
                "Error al intentar accesar a Storage GetSalesExecutives"
            );
        }
    }
}
