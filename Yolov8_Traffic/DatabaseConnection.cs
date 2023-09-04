using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Yolov8_Traffic
{
    public class DatabaseConnnection
    {
        private SqlConnection dc = new SqlConnection("Data Source = (localdb)\\MSSqlLocalDB; Initial catalog = Yolov8_Traffic; Integrated Security=true; TrustServerCertificate=True");
        public SqlConnection GetConnection()
        {
            return this.dc;
        }
    }
}
