using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Diagnostics;
using System.Windows;

using WPFWeather.Exceptions.PersistentData;
using WPFWeather.HostBuilder;
using WPFWeather.Models;
using WPFWeather.Services.DataProvider;
using WPFWeather.Services.NavigationService;
using WPFWeather.Services.Provider;
using WPFWeather.Services.WeatherProvider;
using WPFWeather.Stores;
using WPFWeather.ViewModels;

namespace WPFWeather {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private readonly IHost _host;

        public App() {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices((context, services) => {
                    string? WeatherApiKey = context.Configuration.GetValue<string>("WeatherApiKey");
                    if (WeatherApiKey == null) {
                        throw new MissingMemberException(nameof(WeatherApiKey), "Weather Api Key not found");
                    }
                    services.AddSingleton<IWeatherProvider, OpenWeatherService>((s) => new OpenWeatherService(WeatherApiKey));

                    IPersistentDataManager persistentDataManager = new JsonPersistentDataManager();
                    services.AddSingleton(persistentDataManager);

                    PersistentData? persistentData;
                    try {
                        persistentData = persistentDataManager.GetPersistentData();
                    }
                    catch (PersistentDataReadException e) {
                        Debug.WriteLine("No persistent data found");
                        persistentData = null;
                    }

                    services.AddSingleton((s) => new AppStore(s.GetRequiredService<IWeatherProvider>(), persistentData));

                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton(s => new MainWindow() {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e) {
            _host.Start();

            NavigationService<WeatherHomeViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<WeatherHomeViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e) {
            var appStore = _host.Services.GetRequiredService<AppStore>();
            var persistentDataManager = _host.Services.GetRequiredService<IPersistentDataManager>();
            persistentDataManager.SaveData(new PersistentData() {
                Location = appStore.Location
            });

            _host.Dispose();
            base.OnExit(e);
        }
    }
}