using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace Aprecia.Bussines.Gateway.People.Extesions;

public static class StartupExtensions
{    
    public static void AddBussinessDependencies(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    
        services.AddMediatR(conf =>
        conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
    
}
