using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Aprecia.OnBoarding.Gateway.Test.Extesions
{
    public static class StartupExtensions
    {
        public static void AddBussinessDependencies(this IServiceCollection services) 
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(conf =>
            conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
