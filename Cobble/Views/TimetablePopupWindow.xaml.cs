using Cobble.Messages;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Input;


namespace Cobble.Views
{
    /// <summary>
    /// TimetablePopupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimetablePopupWindow : Window
    {
        public TimetablePopupWindow()
        {
            InitializeComponent();
            Loaded += TimetablePopupWindow_Loaded;
            SizeChanged += TimetablePopupWindow_SizeChanged;
            Closed += (s, e) =>
            {
                WeakReferenceMessenger.Default.Send<TimetablePopupClosedMessage>();
            };
        }

        private void TimetablePopupWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea; 
            this.Top = workArea.Top + (workArea.Height - this.ActualHeight) / 2;
        }

        private void TimetablePopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;
            this.Left = workArea.Right - this.ActualWidth;
            this.Top = workArea.Top + (workArea.Height -  this.ActualHeight) / 2;
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
