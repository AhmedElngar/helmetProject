# 🪖 IoT-Based Helmet for Accident and Alcohol Detection

###  Bachelor Graduation Project – Higher Technological Institute (HTI)
**Authors:** Ahmed El-Nagar & Team (6 members)  
**Year:** 2025  
**Backend:** ASP.NET Core Web API  
**Frontend:** Flutter  
**Database:** Firebase Realtime Database  
**IoT Components:** NodeMCU ESP8266, Accelerometer, GPS, GSM, MQ-3 Alcohol Sensor

---

##  Project Overview
The **IoT-Based Helmet for Accident and Alcohol Detection** is an intelligent safety system designed to monitor the rider’s physical condition and environment in real time.  
The helmet integrates **IoT sensors** such as accelerometers, GPS, GSM, and alcohol sensors to detect **accidents** and **alcohol consumption**.  
If an accident occurs or the alcohol level exceeds a predefined threshold, the system automatically **sends an alert message** with the driver’s location to emergency contacts or an admin dashboard.  
This project enhances **road safety** and **emergency response** by combining **IoT**, **cloud computing**, and **mobile technologies**.

---

##  System Architecture
The system consists of three main components:

1. **IoT Helmet Device:**  
   - Equipped with an **accelerometer** and **MQ-3 alcohol sensor** to detect crash impact and alcohol levels.  
   - Uses **GPS** for location tracking and **GSM/NodeMCU** for real-time communication.

2. **Backend (ASP.NET Core Web API):**  
   - Manages communication between IoT devices, Firebase, and frontend apps.  
   - Handles **authentication**, **data validation**, **alert generation**, and **helmet-driver assignment**.  
   - Implements **JWT authentication** and **role-based authorization** for Admins and Drivers.  
   - Uses **Firebase Realtime Database** for instant data synchronization.

3. **Frontend (Flutter App + Web Dashboard):**  
   - Displays alerts, live status, and driver information.  
   - Provides an interface for admins to monitor helmets, drivers, and incident logs.

---

##  Backend Description
- **Framework:** ASP.NET Core Web API  
- **Architecture:** RESTful  
- **Authentication:** JWT-based with role management (Admin, Driver)  
- **Database:** Firebase Realtime Database  
- **Main Functionalities:**
  - Accident and alcohol detection data processing.  
  - User authentication and role-based access.  
  - Helmet-driver linking and management.  
  - Real-time data handling with Firebase.  
  - Structured error handling and input validation.  

---

##  How It Works
1. The helmet continuously monitors:  
   - **Impact data** (via accelerometer).  
   - **Alcohol level** (via MQ-3 sensor).  
   - **GPS coordinates** for location tracking.  
2. When an accident or alcohol detection is triggered:  
   - The IoT helmet sends sensor data via GSM to the backend API.  
   - The backend processes the event and updates **Firebase Realtime Database**.  
   - An alert is immediately sent to the **admin dashboard** and **emergency contacts**.  
3. The Flutter app displays the driver’s current status:  
   - **Normal**, **Accident Detected**, or **Alcohol Detected**.

---

##  API Endpoints (Examples)
| Method | Endpoint | Description |
|---------|-----------|-------------|
| `POST` | `/api/auth/login` | Authenticate user (Driver/Admin) |
| `GET` | `/api/helmet/data` | Retrieve latest helmet sensor data |
| `POST` | `/api/helmet/assign` | Assign helmet to driver |
| `PUT` | `/api/alert` | Trigger or update accident/alcohol alert |
| `DELETE` | `/api/user/{id}` | Remove driver account |

---

##  Key Features
-  **Accident Detection:** Real-time impact sensing using an accelerometer.  
-  **Alcohol Detection:** Detects driver alcohol levels via MQ-3 sensor.  
-  **GPS Tracking:** Sends location data with each alert.  
-  **Cloud Integration:** Real-time Firebase Realtime Database sync.  
-  **JWT Authentication:** Role-based access control for secure API use.  
-  **Cross-Platform:** Works with Flutter frontend and IoT hardware.  
-  **Scalability:** Can be deployed on cloud platforms (Azure, AWS).  

---

##  Example Alert Workflow
| Event | Sensor | Trigger | Action |
|--------|---------|----------|--------|
| Crash detected | Accelerometer | Sudden impact | Sends “Accident Detected” alert + GPS |
| Alcohol detected | MQ-3 Sensor | Value exceeds threshold | Sends “Driver under influence” alert |
| Normal condition | All sensors stable | — | Updates driver status as “Active” |

---

##  Technologies Used
| Category | Technology |
|-----------|-------------|
| Programming Languages | C#, Dart, C/C++ (Arduino) |
| Backend Framework | ASP.NET Core Web API |
| Database | Firebase Realtime Database |
| Frontend | Flutter |
| IoT Hardware | NodeMCU ESP8266, GPS, GSM, MQ-3, Accelerometer |
| Tools | Visual Studio, Arduino IDE, Firebase Console, Postman |

---

##  Project Structure
/Backend
/Controllers
/Models
/Services
Program.cs
appsettings.json

/Frontend
/lib
/assets
main.dart

/IoT
Helmet_Code.ino
Sensors/
accelerometer.ino
alcohol_sensor.ino


---

##  Future Enhancements
- Integrate AI-based accident prediction model.  
- Use Google Maps API for real-time route visualization.  
- Add SMS/Email emergency notification service.  
- Host backend on Microsoft Azure for 24/7 availability.  

---

##  Demo & Resources
- **Project Report (PDF):** [View Here](https://drive.google.com/file/d/1KB60BqwLuKryOvNcciWFENnevUJUrD7U/view?usp=sharing)  
- **Demo Video:** Coming Soon  

---


##  License
This project was developed as part of the Bachelor’s graduation requirements at the **Higher Technological Institute – Tenth of Ramadan City, Egypt**.  
All rights reserved © 2025.

