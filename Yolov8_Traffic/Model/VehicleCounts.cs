using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yolov8_Traffic.Model
{
    internal class VehicleCounts
    {
        public DateTime dateAndTime { get; set; }
        public int truck { get; set; }
        public int bus { get; set; }
        public int car { get; set; }
        public int motobike { get; set; }
        public int bike { get; set; }

        public VehicleCounts()
        {
        }

        public VehicleCounts(DateTime dateAndTime, int truck, int bus, int car, int motobike, int bike)
        {
            this.dateAndTime = dateAndTime;
            this.truck = truck;
            this.bus = bus;
            this.car = car;
            this.motobike = motobike;
            this.bike = bike;
        }

    }
}
