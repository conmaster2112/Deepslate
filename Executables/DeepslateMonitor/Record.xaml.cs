using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeepslateMonitor
{
    /// <summary>
    /// Interaction logic for Record.xaml
    /// </summary>
    public partial class Record : UserControl
    {
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            "Id", typeof(string),
            typeof(Record)
            );
        public string Id
        {
            get => (string)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }
        public Record()
        {
            InitializeComponent();
        }
    }
}
