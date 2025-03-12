using Aprecia.Domain.Gateway.Test.Dtos;
using Aprecia.OnBoarding.Gateway.Test.Storage;

namespace Aprecia.DataAccess.Gateway.Test.Storge
{
    public class TestStorage : ITestStorage
    {
        private readonly string _url;

        public TestStorage(string url)
        {
            _url = url;
        }
        public async Task<IEnumerable<ResponseTestDto>> GetTest(CancellationToken cancellationToken)
        {
            var url = _url;
            var dummyData = new List<ResponseTestDto>
            {
                new ResponseTestDto { Id = 1, Description = "Test 1" },
                new ResponseTestDto { Id = 2, Description = "Test 2" },
                new ResponseTestDto { Id = 3, Description = "Test 3" }
            };

            return await Task.FromResult(dummyData);
        }
    }
}
