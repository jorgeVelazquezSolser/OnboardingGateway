using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aprecia.OnBoarding.Gateway.SalesExecutive.Extesions;

public static class StartupExtensions
{
    public static void AddBussinessDependencies(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(conf =>
        conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
