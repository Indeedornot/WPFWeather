using WPFWeather.Services.NavigationService;

namespace WPFWeather.Commands;
public class NavigateBackCommand : CommandBase {
    private readonly NavigationBackService _navigationBackService;
    public NavigateBackCommand(NavigationBackService navigationBackService) {
        _navigationBackService = navigationBackService;
    }

    public override bool CanExecute(object parameter) {
        return _navigationBackService.CanNavigate() && base.CanExecute(parameter);
    }

    public override void Execute(object parameter) {
        _navigationBackService.Navigate();
    }
}