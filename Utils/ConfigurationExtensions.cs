using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Utils
{
    public static class ConfigurationExtensions
    {
        public static T GetConfiguration<T>(this IConfiguration config, string? sectionName = null)
            where T : class, new()
        {
            var result = new T();
            sectionName = sectionName ?? typeof(T).GetTypeInfo().Name;
            config.GetSection(sectionName).Bind(result);
            return result;
        }

        public static IServiceCollection AddDerivedFrom<T>(this IServiceCollection services, Func<Type, IServiceCollection> scope, Assembly? assembly = null)
        {
            var derived = typeof(T)
                .GetDerivedTypes(assembly ?? Assembly.GetCallingAssembly()).ToList();

            var filteredDerived = derived.Where(t => t.GetTypeInfo().IsAbstract == false)
                .ToList();

            filteredDerived.ForEach(t => services = scope(t));

            return services;
        }
    }
}
