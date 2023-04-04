using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace HatsuneMiku.Extensions;

public static class OptionsServiceCollectionExtensions
{
    public static IServiceCollection AddOptions<TOptions>(this IServiceCollection services, Action<OptionsBuilder<TOptions>> optionsAction)
        where TOptions : class
    {
        OptionsBuilder<TOptions> options = services.AddOptions<TOptions>();
        optionsAction(options);

        return services;
    }
}