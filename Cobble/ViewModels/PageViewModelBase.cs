using Cobble.Abstractions;
namespace Cobble.ViewModels
{
    public abstract class PageViewModelBase(ThemeViewModel themeViewModel,IWindowService windowService) : ViewModelBase(themeViewModel)
    {
        protected readonly IWindowService _windowService = windowService;
    }
}
