using System;
using System.Collections.Generic;

using WPFWeather.ViewModels;

namespace WPFWeather.Stores;

public class NavigationStore {

    private ViewModelBase? _currentViewModel;
    public ViewModelBase? CurrentViewModel {
        get => _currentViewModel;
        set {
            _currentViewModel?.Dispose();
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public Func<ViewModelBase>? CurrentViewModelCreator { get; set; }
    public Stack<Func<ViewModelBase>> PreviousViewModelCreators { get; set; } = new();

    public event Action CurrentViewModelChanged;

    private void OnCurrentViewModelChanged() {
        CurrentViewModelChanged?.Invoke();
    }
}