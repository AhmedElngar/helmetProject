using System;

namespace HelmetApiProject.Models
{
    public class Alert
    {
        public Guid AlertId { get; set; }
        public string HelmetId { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string Type { get; set; } // e.g., "Accident" or "Drunk Driving"
        public DateTime Timestamp { get; set; }
        public Location Location { get; set; }
        public string Level { get; set; } // e.g., "High", "Medium"
        public string Status { get; set; } // e.g., "Active", "Resolved"
    }
}
