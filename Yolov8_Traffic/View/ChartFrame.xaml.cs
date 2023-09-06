using Microsoft.Data.SqlClient;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Yolov8_Traffic.Controler;
using Yolov8_Traffic.Model;

namespace Yolov8_Traffic.View
{
    /// <summary>
    /// Interaction logic for ChartFrame.xaml
    /// </summary>
    public partial class ChartFrame : Page
    {
        private DateTime? selectedDate;
        private static DatabaseConnection dtc = new DatabaseConnection();
        private static SqlCommand cmd = new SqlCommand();
        public ChartFrame()
        {
            InitializeComponent();
        }
        public void SetSelectedDate(DateTime? selectedDate)
        {
            this.selectedDate = selectedDate;
            ShowPlotView();
        }
        public void ShowPlotView()
        {
            //data get in Database
            var dataPoints = new List<DataPointModel>();
            if (dtc.GetConnection().State == System.Data.ConnectionState.Closed)
            {
                dtc.GetConnection().Open();
            }
            try
            {
                string date = this.selectedDate.Value.Date.ToString("yyyy-MM-dd");

                string querry = "SELECT * FROM VehicleCounts WHERE CONVERT(DATE, dateAndTime) = @date";
                cmd = new SqlCommand(querry, dtc.GetConnection());
                cmd.Parameters.AddWithValue("@date", date);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime dateAndTime = reader.GetDateTime(0);
                    int truck = reader.GetInt32(1);
                    int bus = reader.GetInt32(2);
                    int car = reader.GetInt32(3);
                    int motobike = reader.GetInt32(4);
                    int bike = reader.GetInt32(5);

                    var dataPoint = new DataPointModel
                    {
                        Time = dateAndTime,
                        TDC = bike * 1 + motobike * 1.5 + car * 4 + bus * 8 + truck * 10
                    };
                    dataPoints.Add(dataPoint);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            var plotModel = new PlotModel { Title = "Traffic Density Chart" };
            //set Axis
            var xAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "HH:mm:ss",
                Title = "Time"
            };
            plotModel.Axes.Add(xAxis);
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Title = "TDC"
            };
            plotModel.Axes.Add(yAxis);
            //Set data
            var series = new LineSeries();
            foreach (var dataPoint in dataPoints)
            {
                double xValue = DateTimeAxis.ToDouble(dataPoint.Time);
                series.Points.Add(new DataPoint(xValue, dataPoint.TDC));
            }
            plotModel.Series.Add(series);
            PlotView.Model = plotModel;
        }
    }
}
