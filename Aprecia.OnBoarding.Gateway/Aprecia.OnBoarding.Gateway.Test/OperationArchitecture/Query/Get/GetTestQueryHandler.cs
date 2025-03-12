using Aprecia.Domain.Gateway.Test.Media;
using Aprecia.OnBoarding.Gateway.Test.Base;
using Aprecia.OnBoarding.Gateway.Test.Storage;
using MediatR;

namespace Aprecia.OnBoarding.Gateway.Test.OperationArchitecture.Query.Get
{
    public class GetTestQueryHandler:RequestHandlerBase,IRequestHandler<GetTestQuery, TestResourceList>
    {
        public GetTestQueryHandler(IUnitOfWorkFactory unitOfWorkFactory):base(unitOfWorkFactory) { }

        public async Task<TestResourceList> Handle(GetTestQuery request,CancellationToken cancellationToken) 
        { 
            var result = await UnitOfWorks.TestStorage.GetTest(cancellationToken);

            return new TestResourceList(result);
        }
    }
}
