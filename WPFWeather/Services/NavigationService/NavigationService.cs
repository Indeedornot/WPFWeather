using System;

using WPFWeather.Stores;
using WPFWeather.ViewModels;

namespace WPFWeather.Services.NavigationService;
public class NavigationService<TViewModel> where TViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

    public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate()
    {
        if (_navigationStore.CurrentViewModelCreator != null)
        {
            _navigationStore.PreviousViewModelCreators.Push(_navigationStore.CurrentViewModelCreator);
        }
        _navigationStore.CurrentViewModelCreator = _createViewModel;
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}