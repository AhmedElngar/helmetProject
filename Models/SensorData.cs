using System;
using System.Collections.Generic;

namespace HelmetApiProject.Models
{
    public class SensorData
    {
        public string HelmetId { get; set; }
        public double AlcoholLevel { get; set; }
        public DamageSensorsDTO DamageSensors { get; set; }
        public bool IsWorn { get; set; }
        public DateTime Timestamp { get; set; }
        public Location Location { get; set; }
    }
}
