using WPFWeather.Commands;
using WPFWeather.Services.NavigationService;
using WPFWeather.ViewModels.SetLocation;

namespace WPFWeather.ViewModels;
internal class WelcomeViewModel : ViewModelBase
{
    public NavigateCommand<SetLocationViewModel> SetLocationCommand { get; }
    public WelcomeViewModel(NavigationService<SetLocationViewModel> setLocationNavigationService)
    {
        SetLocationCommand = new NavigateCommand<SetLocationViewModel>(setLocationNavigationService);
    }
}