using System.Windows;
using System.Windows.Controls;


namespace Cobble.Views
{
    /// <summary>
    /// LuckyDrawPage.xaml 的交互逻辑
    /// </summary>
    
    public partial class LuckyDrawPage : UserControl
    {
        private bool _isPulling = false;
        public LuckyDrawPage()
        {
            InitializeComponent();
        }



        private async void Rod_Click(object sender, RoutedEventArgs e)
        {
            if (_isPulling) return;
            _isPulling = true;
            try
            {
                while (RodTranslateTransform.Y < Groove.ActualHeight - Rod.ActualHeight - 5)
                {
                    RodTranslateTransform.Y += 4;
                    await Task.Delay(10);
                }
                RodTranslateTransform.Y = Groove.ActualHeight - Rod.ActualHeight - 5;
                while (RodTranslateTransform.Y > 0)
                {
                    RodTranslateTransform.Y -= 4;
                    await Task.Delay(10);
                }
            }
            finally
            {
                RodTranslateTransform.Y = 0;
                _isPulling = false;
            }
        }
    }
}
