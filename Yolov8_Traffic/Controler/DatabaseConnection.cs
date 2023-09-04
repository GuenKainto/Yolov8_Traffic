using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yolov8_Traffic.Controler
{
    public class DatabaseConnection
    {
        private SqlConnection dc = new SqlConnection("Data Source = LAPTOP-T46EHQ2Q; Initial catalog = Yolov8_Traffic; Integrated Security=true; TrustServerCertificate=True");
        public SqlConnection GetConnection()
        {
            return dc;
        }
    }
}
