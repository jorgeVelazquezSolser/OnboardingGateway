using Aprecia.DI.Gateway.SalesExecutive.Helper;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;
using Microsoft.Extensions.DependencyInjection;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Extesions;
using Microsoft.Extensions.Configuration;


namespace Aprecia.DI.Gateway.SalesExecutive.Extensions;

public static class StartupExtension
{
    public static IServiceCollection ConfigureSalesExecutiveDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionstring = configuration.GetConnectionString("Originacion");
        services.AddBussinessDependencies();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>(provider =>
            new UnitOfWorkFactory(
                connectionstring: connectionstring
        ));

        return services;
    }
}
