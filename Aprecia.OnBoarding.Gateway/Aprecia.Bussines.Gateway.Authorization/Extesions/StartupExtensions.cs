using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Aprecia.Bussines.Gateway.Authorization.Extesions;

public static class StartupExtensions
{
    public static void AddBussinessDependencies(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(conf =>
        conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
