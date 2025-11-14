using System.Reflection;
using FluentValidation;
using MediatR;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Application.Behaviors;
using Venice.Orders.Application.Mappings;

namespace Venice.Orders.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton<IMapper>(mapper);

        return services;
    }
}

