using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelmetApiProject.Models
{
    public class Helmet
    {
        [Key]
        public string HelmetId { get; set; } // Unique integer ID for the helmet, primary key

        public string DriverId { get; set; } // This can be used to link to a driver if needed

        public double AlcoholLevel { get; set; }

        public bool DamageDetected { get; set; }

        public DamageSensorsDTO  DamageSensors { get; set; } // sensor1: 0.3, etc.

        public bool IsWorn { get; set; } // Based on heart rate

        public DateTime Timestamp { get; set; } // Use DateTime for timestamps
    }
}