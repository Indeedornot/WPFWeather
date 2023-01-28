using System.Threading.Tasks;

using WPFWeather.Models.LocationInfo;

namespace WPFWeather.ViewModels.SetLocation;
public interface IViewModelSetLocation {
    public Task<Location?> GetLocation();

    public string? ErrorMessage { get; set; }
    public bool HasError { get; }
}
