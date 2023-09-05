using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Yolov8_Traffic.Controler
{
    internal class VehicleCounts_Controler
    {
        private static DatabaseConnection dtc = new DatabaseConnection();
        private static SqlCommand cmd;
        public static bool Insert(DateTime dateAndTime, int truck, int bus, int car, int motobike, int bike) 
        {
            string querry = "INSERT INTO VehicleCounts VALUES (@dateAndTime,@truck,@bus,@car,@motobike,@bike)";
            if(dtc.GetConnection().State == System.Data.ConnectionState.Closed) 
            {
                dtc.GetConnection().Open();
            }
            try
            {
                cmd = new SqlCommand(querry, dtc.GetConnection());
                cmd.Parameters.AddWithValue("@dateAndTime",dateAndTime);
                cmd.Parameters.AddWithValue("@truck", truck);
                cmd.Parameters.AddWithValue("@bus", bus);
                cmd.Parameters.AddWithValue("@car", car);
                cmd.Parameters.AddWithValue("@motobike", motobike);
                cmd.Parameters.AddWithValue("@bike", bike);
                object result = cmd.ExecuteScalar();
                return true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message,"Error");
                return false;
            }
        }

        

    }
}
