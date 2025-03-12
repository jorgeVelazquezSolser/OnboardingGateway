using Aprecia.OnBoarding.Gateway.Test.Storage;

namespace Aprecia.DataAccess.Gateway.Test.Storge
{
    public class UnitOfWork: IUnitOfWorks
    {
        public readonly string _url;

        public ITestStorage TestStorage {  get;}

        public UnitOfWork(string _url)
        {
            TestStorage = new TestStorage(_url);
        }
    }
}
