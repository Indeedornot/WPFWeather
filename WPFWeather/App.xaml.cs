using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Windows;

using WPFWeather.HostBuilder;
using WPFWeather.Models;
using WPFWeather.Services.DataProvider;
using WPFWeather.Services.LocationProvider;
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
                    services.AddSingleton<IWeatherProvider, MeteoWeatherProvider>();
                    services.AddSingleton<ILocationProvider, GeocodeLocationProvider>();

                    IPersistentDataManager persistentDataManager = new JsonPersistentDataManager();
                    services.AddSingleton(persistentDataManager);

                    services.AddSingleton((s) => new AppStore(s.GetRequiredService<IWeatherProvider>(), persistentDataManager));

                    services.AddSingleton<NavigationBackService>();

                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton(s => new MainWindow() {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e) {
            _host.Start();

            SetInitialView();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e) {
            SavePersistentData();
            _host.Dispose();
            base.OnExit(e);
        }

        private void SetInitialView() {
            IPersistentDataManager persistentDataManager = _host.Services.GetRequiredService<IPersistentDataManager>();
            PersistentData? persistentData = null;
            try {
                persistentData = persistentDataManager.GetPersistentData();
            }
            catch (Exception) { }

            if (persistentData?.Location != null) {
                NavigationService<WeatherHomeViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<WeatherHomeViewModel>>();
                navigationService.Navigate();
            }
            else {
                NavigationService<WelcomeViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<WelcomeViewModel>>();
                navigationService.Navigate();
            }
        }

        private void SavePersistentData() {
            var appStore = _host.Services.GetRequiredService<AppStore>();
            var persistentDataManager = _host.Services.GetRequiredService<IPersistentDataManager>();
            if (appStore.Location != null) {
                persistentDataManager.SaveData(new PersistentData() {
                    Location = appStore.Location
                });
            }
        }
    }
}