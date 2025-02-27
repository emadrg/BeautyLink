using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Utils
{
    public abstract class BaseMapper
    {
        protected IMapper Mapper;

        public BaseMapper(IMapper mapper)
        {
            Mapper = mapper;
        }

        public abstract void Config(IMapperConfigurationExpression config);
    }

    public class MapperService
    {
        private IServiceProvider Services;
        public IMapper AutoMapper { get; set; }
        public MapperService(IServiceProvider services, IMapper mapper)
        {
            Services = services;
            AutoMapper = mapper;
        }

        public D Map<S, D>(S source)
        {
            return AutoMapper.Map<S, D>(source);
        }
        public D Map<S, D>(S source, D destination)
        {
            return AutoMapper.Map<S, D>(source, destination);
        }
    }

    public static class MapperExtensions
    {
        public static IEnumerable<U> Map<T, U>(this MapperService mapperService, IEnumerable<T> collection)
        {
            return mapperService.Map<IEnumerable<T>, IEnumerable<U>>(collection);
        }
        public static List<U> Map<T, U>(this MapperService mapperService, List<T> collection)
        {
            return mapperService.Map<List<T>, List<U>>(collection);
        }
        public static IQueryable<U> Map<T, U>(this MapperService mapperService, IQueryable<T> collection)
        {
            return collection.ProjectTo<U>(mapperService.AutoMapper.ConfigurationProvider);
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mappers = typeof(BaseMapper).GetDerivedTypes(Assembly.GetCallingAssembly())
                .Where(t =>
                    !t.GetTypeInfo().IsInterface &&
                    !t.GetTypeInfo().IsAbstract)
                .ToList();

            var configMethodInterface = typeof(BaseMapper).GetMethods().FirstOrDefault()?.Name;

            var config = new MapperConfiguration(cfg =>
            {
                mappers.ForEach(mapperClass =>
                {
                    var ctorParams = mapperClass
                        .GetConstructors()
                        .FirstOrDefault()?
                        .GetParameters()?
                        .Select(p =>
                        {
                            return default(object);
                        })
                        .ToArray();

                    if (configMethodInterface != null)
                    {
                        mapperClass.GetMethod(configMethodInterface)?.Invoke(Activator.CreateInstance(mapperClass, ctorParams as object[]), new[] { cfg });
                    }
                });
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            return services.AddScoped<MapperService>();
        }
    }
}
