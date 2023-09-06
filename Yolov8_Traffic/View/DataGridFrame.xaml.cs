using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using Yolov8_Traffic.Controler;

namespace Yolov8_Traffic.View
{
    /// <summary>
    /// Interaction logic for DataGridFrame.xaml
    /// </summary>
    public partial class DataGridFrame : Page
    {
        private static DatabaseConnection dtc = new DatabaseConnection();
        private static SqlCommand cmd = new SqlCommand();
        private DateTime? selectedDate;
        public DataGridFrame()
        {
            InitializeComponent();
        }
        public void SetSelectedDate(DateTime? selectedDate)
        {
            this.selectedDate = selectedDate;
            ShowData();
        }
        public void ShowData()
        {
            VehicleCounts_dg.Items.Clear();
            string date = this.selectedDate.Value.Date.ToString("yyyy-MM-dd");

            string querry = "SELECT * FROM VehicleCounts WHERE CONVERT(DATE, dateAndTime) = @date";
            cmd = new SqlCommand(querry, dtc.GetConnection());
            cmd.Parameters.AddWithValue("@date", date);

            if (dtc.GetConnection().State == System.Data.ConnectionState.Closed)
            {
                dtc.GetConnection().Open();
            }
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime dateAndTime = reader.GetDateTime(0);
                    string timeOnly = dateAndTime.ToString("HH:mm:ss");
                    int truck = reader.GetInt32(1);
                    int bus = reader.GetInt32(2);
                    int car = reader.GetInt32(3);
                    int motobike = reader.GetInt32(4);
                    int bike = reader.GetInt32(5);
                    VehicleCounts_dg.Items.Add(new { time = timeOnly, truck = truck, bus = bus, car = car, motobike = motobike, bike = bike, TDC = bike * 1 + motobike * 1.5 + car * 4 + bus * 8 + truck * 10 });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            var temp = VehicleCounts_dg.SelectedItem;
            if (temp != null)
            {
                MessageBoxResult rs = MessageBox.Show("Are you sure to delete this ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rs == MessageBoxResult.Yes)
                {
                    dynamic selected = temp;
                    string date = this.selectedDate.Value.Date.ToString("yyyy-MM-dd");
                    string time = selected.time;
                    DateTime dateAndTime = DateTime.ParseExact(date + " " + time, "yyyy-MM-dd HH:mm:ss", null);

                    bool result = VehicleCounts_Controler.DeleteByDateAndTime(dateAndTime);
                    if (result == true)
                    {
                        MessageBox.Show("Delete successfully", "Message");
                        ShowData();
                    }
                    else MessageBox.Show("Can't delete", "Message");
                }
            }
            else MessageBox.Show("Please choose data you want to delete", "Warning");
        }

        private void Clear_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Are you sure to clear all this ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                bool result = VehicleCounts_Controler.DeleteAll(this.selectedDate.Value);
                if (result == true)
                {
                    MessageBox.Show("Clear successfully", "Message");
                    ShowData();
                }
                else MessageBox.Show("Can't Clear", "Message");
            }
        }
    }
}
