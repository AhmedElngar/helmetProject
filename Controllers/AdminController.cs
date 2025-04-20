using HelmetBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using HelmetApiProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using HelmetApiProject.Models.HelmetApiProject.Models;
using System.Security.Claims;
using BCrypt.Net;
using Newtonsoft.Json.Linq;

namespace HelmetApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public AdminController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // GET: api/admin/alerts
        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            var firebaseAlerts = await _firebaseService.GetAllAsync<JObject>("alerts");

            if (firebaseAlerts == null || firebaseAlerts.Count == 0)
                return NotFound("No alerts found.");

            var alertList = new List<AlertResponseDto>();

            foreach (var entry in firebaseAlerts)
            {
                var raw = entry.Value as JObject;
                if (raw == null) continue;

                var dto = new AlertResponseDto
                {
                    Name = raw["DriverName"]?.ToString(),
                    Id = raw["DriverId"]?.ToString(),
                    Time = raw["Timestamp"]?.ToString(),
                    Severity = raw["Level"]?.ToString(),
                    Location = raw["Location"]?.ToObject<Location>(),
                    AccedientType = raw["Type"]?.ToString()
                };

                alertList.Add(dto);
            }

            return Ok(alertList);
        }

        // GET: api/admin/drivers/count
        [HttpGet("drivers/count")]
        public async Task<IActionResult> CountDrivers()
        {
            // Get all drivers from the "drivers" collection
            var drivers = await _firebaseService.GetAllAsync<Driver>("drivers");
            int count = drivers.Count(); // Get the count of drivers

            return Ok(new { count });
        }

        // GET: api/admin/accidents/count
        [HttpGet("accidents/count")]
        public async Task<IActionResult> CountAccidents()
        {
            // Get all alerts from the "alerts" collection
            var alerts = await _firebaseService.GetAllAsync<Alert>("alerts");

            // Filter the alerts to count only those marked as accidents
            var accidentCount = alerts
                .Where(alert => alert.Value.Type.ToString() == "Accident") // Accessing 'Type' in the 'Alert' object
                .Count();

            return Ok(new { accidentCount });
        }

        //Delete: api/admin/driver
        [HttpDelete("driver/{driverId}")]
        public async Task<IActionResult> DeleteDriverWithHelmet(string driverId)
        {
            var driver = await _firebaseService.GetAsync<Driver>($"drivers/{driverId}");
            if (driver == null)
                return NotFound("Driver not found.");

            // Delete helmet first
            if (!string.IsNullOrEmpty(driver.HelmetId))
            {
                await _firebaseService.DeleteAsync($"helmets/{driver.HelmetId}");
            }

            // Delete driver
            await _firebaseService.DeleteAsync($"drivers/{driverId}");

            return Ok(new { message = "Driver and associated helmet deleted successfully." });
        }

        // PUT: api/Admin/Driver/helmet

        [HttpPut("driver/helmet")]
        public async Task<IActionResult> UpdateDriverHelmet([FromBody] UpdateHelmetDto updateHelmetDto)
        {
            if (updateHelmetDto == null || string.IsNullOrEmpty(updateHelmetDto.DriverId) || string.IsNullOrEmpty(updateHelmetDto.NewHelmetId))
                return BadRequest("DriverId and NewHelmetId are required.");

            var driver = await _firebaseService.GetAsync<Driver>($"drivers/{updateHelmetDto.DriverId}");
            if (driver == null)
                return NotFound("Driver not found.");

            // Delete old helmet if exists
            if (!string.IsNullOrEmpty(driver.HelmetId))
            {
                var oldHelmet = await _firebaseService.GetAsync<Helmet>($"helmets/{driver.HelmetId}");
                if (oldHelmet != null)
                {
                    await _firebaseService.DeleteAsync($"helmets/{driver.HelmetId}");
                }
            }

            // Check if new helmet exists and is assigned
            var newHelmet = await _firebaseService.GetAsync<Helmet>($"helmets/{updateHelmetDto.NewHelmetId}");

            if (newHelmet != null && !string.IsNullOrEmpty(newHelmet.DriverId))
            {
                return BadRequest("New helmet is already assigned to another driver.");
            }

            // If helmet doesn't exist, create it
            if (newHelmet == null)
            {
                newHelmet = new Helmet
                {
                    HelmetId = updateHelmetDto.NewHelmetId,
                    DriverId = driver.Id,
                    Timestamp = DateTime.UtcNow
                };
            }
            else
            {
                newHelmet.DriverId = driver.Id;
            }

            // Update driver
            driver.HelmetId = updateHelmetDto.NewHelmetId;
            await _firebaseService.SetAsync($"drivers/{driver.Id}", driver);

            // Save new or updated helmet
            await _firebaseService.SetAsync($"helmets/{updateHelmetDto.NewHelmetId}", newHelmet);

            return Ok("Helmet updated, new helmet assigned, and old helmet deleted successfully.");
        }




     
        // POST: api/admin/login
        private static readonly string SecretKey = "your-256-bit-secret1234567890123456"; // 256-bit key (32 chars)

        
        // POST: api/Admin/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (login == null)
                return BadRequest("Login data is required.");

            // Get user details from Firebase using the username
            var user = await _firebaseService.GetUserAsync(login.Username);

            // If user is not found, return unauthorized
            if (user == null)
                return Unauthorized("Invalid credentials.");

            // Compare the password directly (without hashing)
            if (login.Password != user.Password)
                return Unauthorized("Invalid credentials.");

            // Generate the JWT token if credentials match
            var token = GenerateJwtToken(login);
            return Ok(new { token });
        }


        private string GenerateJwtToken(LoginModel login)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, login.Username), // Add other claims as needed
            // You can add roles, permissions, etc.
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)); // Use the 256-bit key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Use HMAC SHA256 algorithm

            var token = new JwtSecurityToken(
                issuer: "your-issuer",  // Replace with your actual issuer
                audience: "your-audience", // Replace with your actual audience
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Set token expiration
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Generate the token
        }

        //[HttpPost("seed")]
        //public async Task<IActionResult> SeedAdmin()
        //{
        //    var admin = new
        //    {
        //        username = "admin",
        //        password = "admin123" // hash for "secureadmin123"
        //    };

        //    await _firebaseService.SetAsync("admins/admin", admin);

        //    return Ok("Admin user seeded successfully.");
        //}

    }
}
