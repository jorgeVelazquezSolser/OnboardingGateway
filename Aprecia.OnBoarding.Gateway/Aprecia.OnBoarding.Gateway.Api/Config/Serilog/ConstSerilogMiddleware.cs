namespace Aprecia.OnBoarding.Gateway.Api.Config.Serilog
{
    public class ConstSerilogMiddleware
    {
        public static readonly string[] ALLOWED_PATHS = { Sales_Executive, People };
        public const string Sales_Executive = "/api/SalesExecutive";
        public const string People = "/api/People";
    }
}
