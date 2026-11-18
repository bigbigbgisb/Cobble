
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
namespace Cobble.Views
{
    /// <summary>
    /// EasterEggPage.xaml 的交互逻辑
    /// </summary>
    public partial class EasterEggPage : UserControl
    {
        public EasterEggPage()
        {
            InitializeComponent();
            Loaded += EasterEggPage_Loaded;
        }
        private void EasterEggPage_Loaded(object sender, RoutedEventArgs e)
        {
            using var reader = XmlReader.Create(Path.Combine(AppContext.BaseDirectory, "Assets", "CsharpHighlighting.xshd"));
            CodeEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }
    }
}
