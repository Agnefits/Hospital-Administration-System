# 🏥 Hospital Management System

## 📌 Project Overview
The Hospital Management System is a full-stack web application designed to streamline hospital operations, including user management, appointments, medical records, pharmacy, billing, and logging. This project is developed as part of the **DEPI .NET Fullstack - EYouth Internship (AST2_SWD5_S1)**.

## 👥 Team Members
| Name |  |
|------------|------------|
| **Abdallah Salah Abdallah Mohammed** | 🎓 |
| **Esmail Mohamed Hussein Abady** | 🎓 |
| **Mostafa Mohamed Salah Gabr** | 🎓 |
| **Alaa Youssri Nasr Essa** | 🎓 |
| **Rehab Emad Mohamed Ali** | 🎓 |
| **Amera Mostafa Hussein Ahmed** | 🎓 |

## 🛠️ Technologies Used
- **Frontend:** ASP.NET MVC
- **Backend:** .NET 8 (C#)
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT Authentication
- **API Development:** ASP.NET MVC
- **Testing Frameworks:** xUnit / NUnit
- **Deployment:** Azure / AWS

## 🚀 Features
### 🔹 **User Management**
- User registration & login (JWT authentication)
- Role-based authorization (Admin, Doctor, Nurse, Pharmacist, etc.)

### 🔹 **Appointments**
- Booking and managing doctor appointments
- Appointment status tracking

### 🔹 **Medical Records**
- Doctors can create and manage patient medical records
- Secure and role-based access to medical records

### 🔹 **Billing & Pharmacy**
- Generate and retrieve patient bills
- Manage prescriptions by doctors and pharmacists

### 🔹 **Logging & Security**
- Logging user actions (e.g., logins, record updates)
- Secure API endpoints with role-based access control

## 🗄️ Database Schema
### 📋 **Key Tables**
- **Users** (User authentication and roles)
- **Doctors, Nurses, Admins, Pharmacists** (User roles and related information)
- **Patients** (Patient information and medical history)
- **Reservations** (Appointments management)
- **MedicalRecords** (Diagnosis and treatment records)
- **Billing** (Payment and financial records)
- **Prescriptions** (Medication and pharmacy management)
- **Logs** (User activity tracking)

## ⚙️ Installation & Setup
### **Prerequisites**
- .NET 8 SDK
- Microsoft SQL Server
- Visual Studio

### **Backend Setup**
1. Clone the repository:
   ```sh
   git clone https://github.com/Agnefits/Hospital-Administration-System.git
   cd hospital-management
   ```
2. Install dependencies:
   ```sh
   dotnet restore
   ```
3. Configure database connection in `appsettings.json`
4. Apply migrations and update database:
   ```sh
   dotnet ef database update
   ```
5. Run the backend:
   ```sh
   dotnet run
   ```

## 📡 MVC Routes
### 🔑 **Authentication**
- `GET /Account/Login` - Login Page
- `POST /Account/Login` - Process Login
- `GET /Account/Register` - Registration Page
- `POST /Account/Register` - Process Registration

### 👤 **Users & Roles**
- `GET /Users` - View all users
- `GET /Users/Create` - Create new user form
- `POST /Users/Create` - Add new user
- `GET /Users/Edit/{id}` - Edit user form
- `POST /Users/Edit/{id}` - Update user
- `POST /Users/Delete/{id}` - Delete user

### 🏥 **Appointments**
- `GET /Reservations` - View all appointments
- `GET /Reservations/Book` - Book an appointment form
- `POST /Reservations/Book` - Submit appointment
- `POST /Reservations/Cancel/{id}` - Cancel appointment

### 📄 **Medical Records**
- `GET /MedicalRecords` - View all records
- `GET /MedicalRecords/Create` - Create new record form
- `POST /MedicalRecords/Create` - Submit medical record

### 💰 **Billing**
- `GET /Billing` - View patient bills
- `GET /Billing/Create` - Generate new bill form
- `POST /Billing/Create` - Submit bill

## ☁️ Deployment
1. Set up environment variables for production.
2. Deploy the backend to Azure / AWS.
3. Configure database connection for production.
4. Deploy MVC frontend using IIS or cloud hosting.

## 📜 License
This project is open-source and available for educational and professional use. Contributions are welcome!

## 📧 Contact
For any inquiries or contributions, feel free to reach out to the project team members.

