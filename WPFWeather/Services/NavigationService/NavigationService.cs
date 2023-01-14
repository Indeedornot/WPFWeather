﻿using System;

using WPFWeather.Stores;
using WPFWeather.ViewModels;

namespace WPFWeather.Services.NavigationService;
public class NavigationService<TViewModel> where TViewModel : ViewModelBase {
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

    public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel) {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate() {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}