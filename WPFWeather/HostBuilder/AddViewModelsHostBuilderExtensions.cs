﻿using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WPFWeather.Services.NavigationService;
using WPFWeather.ViewModels;
using WPFWeather.ViewModels.SetLocation;

namespace WPFWeather.HostBuilder;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<WeatherHomeViewModel>();
            services.AddSingleton<Func<WeatherHomeViewModel>>((s) => () => s.GetRequiredService<WeatherHomeViewModel>());
            services.AddSingleton<NavigationService<WeatherHomeViewModel>>();

            services.AddTransient<WelcomeViewModel>();
            services.AddSingleton<Func<WelcomeViewModel>>((s) => () => s.GetRequiredService<WelcomeViewModel>());
            services.AddSingleton<NavigationService<WelcomeViewModel>>();

            services.AddTransient<SetLocationViewModel>();
            services.AddSingleton<Func<SetLocationViewModel>>((s) => () => s.GetRequiredService<SetLocationViewModel>());
            services.AddSingleton<NavigationService<SetLocationViewModel>>();

            services.AddSingleton<MainViewModel>();
        });

        return hostBuilder;
    }
}