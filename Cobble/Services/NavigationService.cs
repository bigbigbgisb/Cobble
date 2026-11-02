using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Cobble.Abstractions;
using Microsoft.Extensions.Logging;

namespace Cobble.Services
{
    public partial class NavigationService(ILogger<NavigationService> logger) : ObservableObject, INavigationService
    {
        [ObservableProperty]
        ViewModelBase? _currentPage;
        readonly ILogger<NavigationService> _logger = logger;
        public void NavigateTo<T>() where T : ViewModelBase
        {

            try
            {
                var vm = App.Services?.GetRequiredService<T>();
                if (vm is null)
                {
                    _logger.LogInformation("Service is null");
                    return;
                }
                CurrentPage = vm;
                _logger.LogInformation("Navigate to {ViewModel} (viewmodel)", typeof(T).FullName);
            }
            catch (InvalidOperationException)
            {
                _logger.LogError("Cannot find viewmodel {ViewModel}", typeof(T).FullName);
            }


        }

    }
}
