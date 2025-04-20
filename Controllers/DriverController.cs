using HelmetBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using HelmetApiProject.Models;

namespace HelmetApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public DriverController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // POST: api/driver/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterDriver([FromBody] Driver driver)
        {
            if (driver == null)
                return BadRequest("Driver data is required.");

            if (string.IsNullOrWhiteSpace(driver.HelmetId))
                return BadRequest("HelmetId is required.");

            // Check if this helmet is already assigned
            var allDrivers = await _firebaseService.GetAllAsync<Driver>("drivers");
            if (allDrivers != null)
            {
                foreach (var existingDriver in allDrivers.Values)
                {
                    if (!string.IsNullOrEmpty(existingDriver.HelmetId) &&
                        existingDriver.HelmetId == driver.HelmetId)
                    {
                        return Conflict("This helmet is already assigned to another driver.");
                    }
                    // Email duplication check
                    if (!string.IsNullOrEmpty(existingDriver.Email) &&
                        existingDriver.Email.Equals(driver.Email, StringComparison.OrdinalIgnoreCase))
                    {
                        return Conflict("This email is already registered");
                    }

                    if (!string.IsNullOrEmpty(existingDriver.PhoneNumber) &&
                        existingDriver.PhoneNumber.Equals(driver.PhoneNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        return Conflict("This Phone is already registered");
                    }
                    if (!string.IsNullOrEmpty(existingDriver.LicenseNumber) &&
                       existingDriver.LicenseNumber.Equals(driver.LicenseNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        return Conflict("This License Number is already registered");
                    }

                }
            }


            driver.Id = Guid.NewGuid().ToString();

            // Save the driver
            await _firebaseService.SetAsync($"drivers/{driver.Id}", driver);

            // Save the corresponding helmet
            var helmet = new Helmet
            {
                HelmetId = driver.HelmetId,
                DriverId = driver.Id,
                AlcoholLevel = 0,
                DamageDetected = false,
                DamageSensors = new DamageSensorsDTO(),
                IsWorn = false,
                Timestamp = DateTime.UtcNow
            };

            await _firebaseService.SetAsync($"helmets/{helmet.HelmetId}", helmet);

            // Create and return a DTO
            var driverDto = new DriverRegistrationDto
            {
                Name = driver.Name,
                Email = driver.Email,
                LicenseNumber = driver.LicenseNumber,
                HelmetId = driver.HelmetId,
                Age = driver.Age,
                BloodType = driver.BloodType,
                PhoneNumber = driver.PhoneNumber,
                RelativePhoneNumber = driver.RelativePhoneNumber
            };

            return Ok(new { message = "Driver registered successfully", driver = driverDto });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Email and password are required.");

            var allDrivers = await _firebaseService.GetAllAsync<Driver>("drivers");

            if (allDrivers == null || allDrivers.Count == 0)
                return Unauthorized("No drivers found.");

            foreach (var driver in allDrivers.Values)
            {
                if (driver.Email == login.Email && driver.Password == login.Password)
                {
                    var response = new
                    {
                        message = "Login successful",
                        user = new
                        {
                            driver.Id,
                            driver.Name,
                            driver.Email,
                            driver.HelmetId,
                            Role = "driver"
                        }
                    };

                    return Ok(response);
                }
            }

            return Unauthorized("Invalid email or password.");
        }



        // GET: api/driver/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _firebaseService.GetAllAsync<Driver>("drivers");
            if (drivers == null || drivers.Count == 0)
                return NotFound("No drivers found.");

            var driverDtos = new List<DriverWithIdDto>();
            foreach (var entry in drivers)
            {
                var driver = entry.Value;
                var driverDto = new DriverWithIdDto
                {
                    Id = entry.Key,
                    Name = driver.Name,
                    Email = driver.Email,
                    LicenseNumber = driver.LicenseNumber,
                    HelmetId = driver.HelmetId,
                    Age = driver.Age,
                    BloodType = driver.BloodType,
                    PhoneNumber = driver.PhoneNumber,
                    RelativePhoneNumber = driver.RelativePhoneNumber
                };

                driverDtos.Add(driverDto);
            }

            return Ok(driverDtos);
        }


        // GET: api/driver/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Driver ID is required.");

            var driver = await _firebaseService.GetAsync<Driver>($"drivers/{id}");
            if (driver == null)
                return NotFound("Driver not found.");

            return Ok(driver);
        }
    }
}
