using Cobble.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using System.Windows;

namespace Cobble.Views
{
    /// <summary>
    /// MemoPage.xaml 的交互逻辑
    /// </summary>
    public partial class MemoPage : System.Windows.Controls.UserControl
    {
        public MemoPage()
        {
            InitializeComponent();
        }

        private void ExportMemo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                string folderPath = dialog.FolderName;
                WeakReferenceMessenger.Default.Send(new ExportMemoMessage(folderPath));
            }
            
        }

        private void LoadMemo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = "文本文档(.txt)|*.txt" };
            
            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                WeakReferenceMessenger.Default.Send(new MemoFilePickedMessage(filePath));
            }
        }
    }
}
