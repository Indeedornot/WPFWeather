using WPFWeather.Stores;
using WPFWeather.ViewModels;

namespace WPFWeather.Services.NavigationService;
public class NavigationBackService : ViewModelBase {
    private readonly NavigationStore _navigationStore;

    public NavigationBackService(NavigationStore navigationStore) {
        _navigationStore = navigationStore;
    }

    public bool CanNavigate() {
        return _navigationStore.PreviousViewModelCreators.Count > 0;
    }

    public void Navigate() {
        _navigationStore.PreviousViewModelCreators.TryPop(out var previousViewModelCreator);
        if (previousViewModelCreator != null) {
            _navigationStore.CurrentViewModelCreator = previousViewModelCreator;
            _navigationStore.CurrentViewModel = previousViewModelCreator();
        }
    }
}