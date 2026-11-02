using System.ComponentModel;

namespace Cobble.Abstractions
{
    public interface INavigationService : INotifyPropertyChanged
    {
        ViewModelBase? CurrentPage { get; }
        void NavigateTo<T>() where T : ViewModelBase;
    }
}
