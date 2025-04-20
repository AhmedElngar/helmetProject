using HelmetBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HelmetApiProject.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HelmetApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public SensorController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // POST: api/sensors/data
        [HttpPost("data")]
        public async Task<IActionResult> ReceiveSensorData([FromBody] SensorData sensorData)
        {
            if (sensorData == null)
                return BadRequest("Sensor data is required.");

            if (string.IsNullOrWhiteSpace(sensorData.HelmetId))
                return BadRequest("HelmetId is required.");

            // Check if helmet exists
            var helmet = await _firebaseService.GetAsync<Helmet>($"helmets/{sensorData.HelmetId}");
            if (helmet == null)
                return NotFound("Helmet not found.");

            // Update the helmet with new sensor data
            helmet.AlcoholLevel = sensorData.AlcoholLevel;
            helmet.DamageSensors = sensorData.DamageSensors;
            helmet.IsWorn = sensorData.IsWorn;
            helmet.Timestamp = sensorData.Timestamp;

            // Detect if there is an accident (damage sensor reaches 80 or more)
            bool isAccident = DetectAccident(sensorData.DamageSensors);
            bool isDrunk = sensorData.AlcoholLevel > 0.08;

            if (isAccident || isDrunk)
            {
                var driver = await _firebaseService.GetAsync<Driver>($"drivers/{helmet.DriverId}");

                var alert = new Alert
                {
                    AlertId = Guid.NewGuid(),
                    HelmetId = sensorData.HelmetId,
                    DriverId = helmet.DriverId,
                    DriverName = driver?.Name,
                    Type = isAccident ? "Accident" : "Drunk Driving",  // Use strings or defined enums
                    Timestamp = sensorData.Timestamp,
                    Location = sensorData.Location,
                    Level = isAccident ? "High" : "Medium",  // Use strings or defined enums
                    Status = "Active"  // Set to active by default
                };

                // Save the alert in the "alerts" collection
                await _firebaseService.SetAsync($"alerts/{alert.AlertId}", alert);
            }

            // Save the updated helmet data
            await _firebaseService.SetAsync($"helmets/{sensorData.HelmetId}", helmet);

            return Ok(new { message = "Sensor data received and processed successfully" });
        }

        private bool DetectAccident(DamageSensorsDTO damageSensors)
        {
            return damageSensors.Sensor1 >= 80 ||
                   damageSensors.Sensor2 >= 80 ||
                   damageSensors.Sensor3 >= 80 ||
                   damageSensors.Sensor4 >= 80;
        }

    }
}
