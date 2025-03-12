using Aprecia.DI.Gateway.Test.Helper;
using Aprecia.OnBoarding.Gateway.Test.Extesions;
using Aprecia.OnBoarding.Gateway.Test.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aprecia.DI.Gateway.Test.Extensions;

public static class StartupExtension
{
    public static IServiceCollection ConfigureTestDependencies(this IServiceCollection services,IConfiguration configuration) 
    {
        
        var url = configuration["SomeService:Url"];
        services.AddBussinessDependencies();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>(provider =>
            new UnitOfWorkFactory(
                url: url
        ));

        return services;
    }
}
