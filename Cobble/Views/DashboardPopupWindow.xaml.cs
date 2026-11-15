using Cobble.Messages;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cobble.Views
{
    /// <summary>
    /// DashboardPopupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardPopupWindow : Window
    {
        public DashboardPopupWindow()
        {
            InitializeComponent();
            SizeChanged += DashboardPopupWindow_SizeChanged;
            Loaded += DashboardPopupWindow_Loaded;
            Closed += DashboardPopupWindow_Closed;
        }

        private void DashboardPopupWindow_Closed(object? sender, EventArgs e)
        {
            WeakReferenceMessenger.Default.Send<DashboardPopupClosedMessage>();
        }

        private void DashboardPopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send<PullDashboardMemoInfoMessage>();
            WeakReferenceMessenger.Default.Send<PullDashboardCurrentAndNextLessonMessage>();
            var workArea = SystemParameters.WorkArea;
            this.Left = 0;
            this.Top = workArea.Top + (workArea.Height - this.ActualHeight) / 2;
        }

        private void DashboardPopupWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;
            this.Left = 0;
            this.Top = workArea.Top + (workArea.Height - this.ActualHeight) / 2;
        }


        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
