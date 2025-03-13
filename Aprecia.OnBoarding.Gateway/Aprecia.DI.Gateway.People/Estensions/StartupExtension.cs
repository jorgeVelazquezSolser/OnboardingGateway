using Aprecia.Bussines.Gateway.People.Extesions;
using Aprecia.Bussines.Gateway.People.Storage;
using Aprecia.DI.Gateway.People.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aprecia.DI.Gateway.People.Estensions;

public static class StartupExtension
{
    public static IServiceCollection ConfigurePeopleDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        var url = configuration["SomeService:Originacion"];
        services.AddBussinessDependencies();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>(provider =>
            new UnitOfWorkFactory(
                url: url
        ));

        return services;
    }
}
