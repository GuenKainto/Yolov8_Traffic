using System;
using System.Windows;
using System.Windows.Controls;
using Yolov8_Traffic.View;

namespace Yolov8_Traffic
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        private DateTime? selectedDate;
        public Statistics()
        {
            InitializeComponent();
            DateTime currentTime = DateTime.Now;
            Date_Picker.SelectedDate = DateTime.Now;
            this.selectedDate = Date_Picker.SelectedDate;

            NavigateToDataGridFrame();
        }
        private void Date_Picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedDate = Date_Picker.SelectedDate;
            if (Frame_frm.Content is View.DataGridFrame dataGridFrame)
            {
                NavigateToDataGridFrame();
            }
            else NavigateToChartFrame();
        }
        private void ChangePage_btn_Click(object sender, RoutedEventArgs e)
        {
            this.selectedDate = Date_Picker.SelectedDate;
            if (Frame_frm.Content is View.DataGridFrame dataGridFrame)
            {
                NavigateToChartFrame();
            }
            else NavigateToDataGridFrame();

        }
        private void NavigateToDataGridFrame()
        {
            View.DataGridFrame dataGridFrame = new View.DataGridFrame();
            dataGridFrame.SetSelectedDate(this.selectedDate);
            Frame_frm.Content = dataGridFrame;
        }
        private void NavigateToChartFrame()
        {
            View.ChartFrame chartFrame = new View.ChartFrame();
            chartFrame.SetSelectedDate(this.selectedDate);
            Frame_frm.Content = chartFrame;
        }
    }
}
