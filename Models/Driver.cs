using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Not Newtonsoft.Json unless you're using it explicitly

namespace HelmetApiProject.Models
{
    public class Driver
    {
        [Key]
        [Required]
        public string Id { get; set; } // Unique integer ID for the driver

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; } // Ensure this is hashed and salted

        [Required]
        [StringLength(20)]
        public string LicenseNumber { get; set; }

        [Required]
        [ForeignKey("Helmet")]
        public string HelmetId { get; set; } // Foreign key referencing Helmet

        [Range(18, 60)]
        public int Age { get; set; }

        [StringLength(3)]
        public string BloodType { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

       [Phone]
        public string RelativePhoneNumber { get; set; }

        // Navigation property
        [JsonIgnore]
        public Helmet? Helmet { get; set; }
    }
}