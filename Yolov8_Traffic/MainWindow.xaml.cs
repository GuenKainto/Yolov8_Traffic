using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Yolov8_Traffic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Page_frm.Content = new Detection();
        }

        private void Detection_btn_Click(object sender, RoutedEventArgs e)
        {
            Page_frm.Content = new Detection();
        }

        private void Satistics_btn_Click(object sender, RoutedEventArgs e)
        {
            Page_frm.Content = new Statistics();
        }
    }
}
