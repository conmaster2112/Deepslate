using MicaWPF.Core.Enums;
using MicaWPF.Core.Services;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeepslateMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MicaWPF.Lite.Controls.MicaWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RunTest();
        }
        public async void RunTest()
        {
            /*
            while (true)
            {
                await Task.Delay(500);
                TextBlock textBlock = new();
                textBlock.Text = DateTime.Now.ToLongTimeString();
                Records.Children.Add(textBlock);
                RecordsScrollViewer.ScrollToBottom();
            }*/
        }
    }
}