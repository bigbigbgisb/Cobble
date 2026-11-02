using Cobble.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
