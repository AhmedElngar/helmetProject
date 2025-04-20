namespace HelmetApiProject.Models
{
    public class DriverRegistrationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LicenseNumber { get; set; }
        public string HelmetId { get; set; }
        public int Age { get; set; }
        public string BloodType { get; set; }
        public string PhoneNumber { get; set; }
        public string RelativePhoneNumber { get; set; }
    }
}