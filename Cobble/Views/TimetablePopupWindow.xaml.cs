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
    /// TimetablePopupWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimetablePopupWindow : Window
    {
        public TimetablePopupWindow()
        {
            InitializeComponent();
            Loaded += TimetablePopupWindow_Loaded;
            SizeChanged += TimetablePopupWindow_SizeChanged;
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
    }
}
