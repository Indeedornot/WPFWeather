using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

using WPFWeather.Services.NavigationService;
using WPFWeather.ViewModels;

namespace WPFWeather.HostBuilder;

public static class AddViewModelsHostBuilderExtensions {
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder) {
        hostBuilder.ConfigureServices(services => {
            services.AddTransient<WeatherHomeViewModel>();
            services.AddSingleton<Func<WeatherHomeViewModel>>((s) => () => s.GetRequiredService<WeatherHomeViewModel>());
            services.AddSingleton<NavigationService<WeatherHomeViewModel>>();

            services.AddSingleton<MainViewModel>();
        });

        return hostBuilder;
    }
}
