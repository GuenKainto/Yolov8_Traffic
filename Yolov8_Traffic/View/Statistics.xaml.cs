using Microsoft.Data.SqlClient;
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
using Yolov8_Traffic.Controler;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Yolov8_Traffic
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        private static DatabaseConnection dtc = new DatabaseConnection();
        private static SqlCommand cmd = new SqlCommand();
        public Statistics()
        {
            InitializeComponent();
            DateTime currentTime = DateTime.Now;
            Date_Picker.SelectedDate = DateTime.Now;
            ShowData();
        }

        public void ShowData()
        {
            VehicleCounts_dg.Items.Clear();
            DateTime? selectedDate = Date_Picker.SelectedDate;
            string date = selectedDate.Value.Date.ToString("yyyy-MM-dd");

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
                    VehicleCounts_dg.Items.Add(new { time = timeOnly, truck = truck , bus = bus, car = car, motobike = motobike , bike = bike });
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
                MessageBoxResult rs = MessageBox.Show("Are you sure to delete this ?","Warning",MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rs == MessageBoxResult.Yes)
                {
                    dynamic selected = temp;
                    DateTime? selectedDate = Date_Picker.SelectedDate;
                    string date = selectedDate.Value.Date.ToString("yyyy-MM-dd");
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

                DateTime? selectedDate = Date_Picker.SelectedDate;

                bool result = VehicleCounts_Controler.DeleteAll(selectedDate.Value);
                if (result == true)
                {
                    MessageBox.Show("Clear successfully", "Message");
                    ShowData();
                }
                else MessageBox.Show("Can't Clear", "Message");
            }
        }

        private void Date_Picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowData();
        }
    }
}
