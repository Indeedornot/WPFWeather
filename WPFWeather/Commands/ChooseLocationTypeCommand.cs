using WPFWeather.ViewModels.SetLocation;

namespace WPFWeather.Commands;
public class ChooseLocationTypeCommand : CommandBase {
    private readonly SetLocationViewModel _viewModel;
    public ChooseLocationTypeCommand(SetLocationViewModel viewModel) {
        _viewModel = viewModel;
    }

    public override void Execute(object parameter) {
        _viewModel.LocationType = (string)parameter;
    }
}
