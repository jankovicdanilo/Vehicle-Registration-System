# Vehicle Registration System

Full-stack web application for managing vehicle registrations, clients, insurance policies, and administrative workflows.

Designed as an enterprise-style system with clearly separated backend and frontend layers.

---

## 🏗 Architecture

The application follows a layered architecture:

- Presentation Layer (Angular Frontend)
- API Layer (ASP.NET Core Web API)
- Service Layer (Business Logic)
- Repository Layer (Data Access)
- Database Layer (SQL Server)

---

## 🔧 Tech Stack

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Role-Based Authorization
- RESTful API Design

### Frontend
- Angular
- TypeScript
- Angular Services
- HTTP Client (API communication)
- Reactive Forms

---

## 🔐 Features

- User registration & login
- Role-based access control (Admin / Employee / Manager)
- CRUD operations for:
  - Clients
  - Vehicles
  - Insurance Policies
  - Registrations
- Secure API endpoints
- Database integration
- Clean separation of concerns
- Structured DTO usage

---

## 📂 Project Structure

/backend     → ASP.NET Core Web API  
/frontend    → Angular application  

---

## 🚀 How to Run

### Backend
1. Clone the repository  
2. Configure the connection string in `appsettings.json`  
3. Run:
```
dotnet run
```

### Frontend
1. Navigate to frontend folder:
```
cd frontend
```
2. Install dependencies:
```
npm install
```
3. Start Angular application:
```
ng serve
```

---

## 📈 Purpose of the Project

This project demonstrates:

- Full-stack development skills
- Backend system design
- REST API implementation
- Secure authentication with JWT
- Database modeling and integration
- Clean and layered architecture principles

---

## 👤 Author

Danilo Jankovic  
Software Developer  
GitHub: https://github.com/jankovicdanilo
