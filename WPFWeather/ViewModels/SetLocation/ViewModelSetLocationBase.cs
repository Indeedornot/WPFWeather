using System.Threading.Tasks;

using WPFWeather.Models.LocationInfo;

namespace WPFWeather.ViewModels.SetLocation;
public interface IViewModelSetLocation {
    public Location Location { get; }
    public bool IsValidLocation { get; set; }
    public Task<bool> ValidateLocation();
}
