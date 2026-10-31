using Cobble.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cobble
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;

        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
                Maximize.Content = WindowState == WindowState.Maximized
                            ? "⧉"
                            : "☐";
                return;
            }
            if (WindowState == WindowState.Maximized) return;
            e.Handled = true;
            DragMove();       
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                        ? WindowState.Normal
                        : WindowState.Maximized;
            Maximize.Content = WindowState == WindowState.Maximized
                        ? "⧉"
                        : "☐";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false;
            Hide();
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = true;
            Show();
            Activate();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = true;
            Show();
            Activate();
        }

        private void CloseItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}