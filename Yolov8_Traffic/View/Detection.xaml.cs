using Compunet.YoloV8;
using Microsoft.Win32;
using System;
using System.Collections;
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
using Compunet.YoloV8.Plotting;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.IO;
using System.Runtime.CompilerServices;
using System.Data;
using Microsoft.Data.SqlClient;
using Yolov8_Traffic.Controler;

namespace Yolov8_Traffic
{
    /// <summary>
    /// Interaction logic for Detection.xaml
    /// </summary>
    public partial class Detection : Page
    {
        DatabaseConnection dtc = new DatabaseConnection(); 
        SqlCommand cmd = new SqlCommand();
        public static string selectedImagePath = "" ;
        public static string modelpath = "D:\\[Study]\\.NET\\NET_Programming\\Yolov8_Traffic\\Yolov8_Traffic\\Yolov8_opset15.onnx";
        public Detection()
        {
            InitializeComponent();
            DateTime currentTime = DateTime.Now;
            Date_Picker.SelectedDate = DateTime.Now;
            string formattedTime = currentTime.ToString("HH:mm:ss");
            Time_tb.Text = formattedTime;
        }

        private void Add_Image_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.png;*.jpeg;*.gif)|*.jpg;*.png;*.jpeg;*.gif|All Files (*.*)|*.*";
        
            if(openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedImagePath);
                bitmapImage.EndInit();

                Add_img.Source = bitmapImage;
            }
        }

        private void Detect_btn_Click(object sender, RoutedEventArgs e)
        {
            using var predictor = new YoloV8(modelpath);
            var result = predictor.Detect(selectedImagePath);
            Test.Text = result.ToString();
            Dictionary<string, int> vehicleCounts = ParseVehicleCounts(result.ToString());

            string selectedKey1 = "Truck";
            string selectedKey2 = "Bus";
            string selectedKey3 = "Car";
            string selectedKey4 = "Motobike";
            string selectedKey5 = "Bike";
            if (vehicleCounts.ContainsKey(selectedKey1) || vehicleCounts.ContainsKey(selectedKey2) || 
                vehicleCounts.ContainsKey(selectedKey3) || vehicleCounts.ContainsKey(selectedKey4) || 
                vehicleCounts.ContainsKey(selectedKey5))
            {
                int selectedValue1 = vehicleCounts[selectedKey1];
                int selectedValue2 = vehicleCounts[selectedKey2];
                int selectedValue3 = vehicleCounts[selectedKey3];
                int selectedValue4 = vehicleCounts[selectedKey4];
                int selectedValue5 = vehicleCounts[selectedKey5];
                Trucks_txb.Text = selectedValue1.ToString();
                Buses_txb.Text = selectedValue2.ToString();
                Cars_txb.Text = selectedValue3.ToString();
                Motobikes_txb.Text = selectedValue4.ToString();
                Bikes_txb.Text = selectedValue5.ToString();
            }
            

            //plotting image
            using var origin = SixLabors.ImageSharp.Image.Load<Rgb24>(selectedImagePath);
            using var ploted = result.PlotImage(origin);

            //Save into memory to change ImageShape into bitmap
            using var ms = new MemoryStream();
            ploted.SaveAsPng(ms);
            ms.Position = 0;
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            Detect_img.Source = bitmap;
            ms.Dispose();

            /* ----------SAVE RESULT--------------
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    ploted.Save(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi lưu hình ảnh: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }*/


        }

        static Dictionary<string, int> ParseVehicleCounts(string input)
        {
            Dictionary<string, int> vehicleCounts = new Dictionary<string, int>();

            // Split string : string look like  3 truck, 1 bus, 2 car 
            string[] parts = input.Split(',');

            foreach (string part in parts)
            {
                string[] keyValue = part.Trim().Split(' ');
                if (keyValue.Length == 2)
                {
                    string vehicle = keyValue[1];
                    int count;
                    if (int.TryParse(keyValue[0], out count))
                    {
                        vehicleCounts.Add(vehicle, count);
                    }
                }
            }

            if (!vehicleCounts.ContainsKey("Truck"))    vehicleCounts.Add("Truck", 0);        
            if (!vehicleCounts.ContainsKey("Bus"))      vehicleCounts.Add("Bus", 0);        
            if (!vehicleCounts.ContainsKey("Car"))      vehicleCounts.Add("Car", 0);
            if (!vehicleCounts.ContainsKey("Motobike")) vehicleCounts.Add("Motobike", 0);
            if (!vehicleCounts.ContainsKey("Bike"))     vehicleCounts.Add("Bike", 0);

            return vehicleCounts;
        }

        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {
            string date = Date_Picker.SelectedDate.ToString();
            string time = Time_tb.Text;
            DateTime dateAndTime = DateTime.ParseExact(date + " " + time, "yyyy-MM-dd HH:mm:ss", null);

            int truck = Int32.Parse(Trucks_txb.Text);
            int bus = Int32.Parse(Buses_txb.Text);
            int car = Int32.Parse(Cars_txb.Text);
            int motobike = Int32.Parse(Motobikes_txb.Text);
            int bike = Int32.Parse(Bikes_txb.Text);

            bool result = VehicleCounts_Controler.Insert(dateAndTime,truck, bus, car,motobike,bike);
            if (result == true) MessageBox.Show("Save data successfully","Message");
            else MessageBox.Show("Can't save","Message");
        }
    }
}
