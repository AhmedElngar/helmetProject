namespace HelmetApiProject.Models
{
    public class AlertResponseDto
    {

        public string Name { get; set; }
        public string Id { get; set; }
        public string Time { get; set; }
        public string Severity { get; set; }
        public string AccedientType { get; set; }


        public Location Location { get; set; }

    }
}
