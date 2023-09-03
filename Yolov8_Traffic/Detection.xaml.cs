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

namespace Yolov8_Traffic
{
    /// <summary>
    /// Interaction logic for Detection.xaml
    /// </summary>
    public partial class Detection : Page
    {
        public static string selectedImagePath ;
        public static string modelpath = "D:\\[Study]\\.NET\\NET_Programming\\Yolov8_Traffic\\Yolov8_Traffic\\Yolov8_opset15.onnx";
        public Detection()
        {
            InitializeComponent();
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


        private void Save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
