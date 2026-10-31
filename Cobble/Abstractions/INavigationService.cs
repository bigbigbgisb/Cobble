using System.ComponentModel;

namespace Cobble.Abstactions
{
    public interface INavigationService : INotifyPropertyChanged
    {
        ViewModelBase? CurrentPage { get; }
        void NavigateTo<T>() where T : ViewModelBase;
    }
}
