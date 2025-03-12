using Aprecia.Bussines.Gateway.Authorization.Storage;
using Aprecia.DI.Gateway.Authorization.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aprecia.Bussines.Gateway.Authorization.Extesions;

namespace Aprecia.DI.Gateway.Authorization.Extensions;

public static class StartupExtension
{
    public static IServiceCollection ConfigureAuthorizationDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionstring = configuration.GetConnectionString("OnboardingGateway");
        services.AddBussinessDependencies();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>(provider =>
            new UnitOfWorkFactory(
                connectionstring: connectionstring
        ));

        return services;
    }
}
