# Vehicle Registration System

Full-stack web application for managing vehicle registrations, clients, insurance policies, and administrative workflows.

Designed as an enterprise-style system with clearly separated backend and frontend layers, focusing on clean architecture, scalability, and real-world backend practices.

---

## 📋 Prerequisites

* .NET 8 SDK
* SQL Server
* Node.js & npm
* Angular CLI (`npm install -g @angular/cli`)

---

## 🏗 Architecture

The application follows a layered architecture:

* Database Layer (SQL Server)
* Repository Layer (Data Access)
* Service Layer (Business Logic)
* API Layer (ASP.NET Core Web API)
* Presentation Layer (Angular Frontend)
 
Additional architectural patterns:

* Repository Pattern with generic `RepositoryBase`
* DTO-based data transfer
* Centralized validation logic

---

## 🔧 Tech Stack

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* Role-Based Authorization
* RESTful API Design

### Frontend

* Angular
* TypeScript
* Angular Services
* HTTP Client (API communication)
* Reactive Forms

---

## 🔐 Features

* User registration & login
* Role-based access control (Admin / Employee / Manager)
* CRUD operations for:

  * Clients
  * Vehicles
  * Insurance Policies
  * Registrations
* Secure API endpoints
* Clean separation of concerns
* Structured DTO usage

---

## ⚙️ Backend Highlights

* Generic `RepositoryBase` implementation to reduce duplication
* Service layer responsible for business logic and validation
* Advanced validation rules (cross-entity checks such as vehicle-brand-type consistency)
* Concurrency-safe operations using database constraints
* Unique constraints:

  * One registration per vehicle
  * Unique license plate enforcement
* Graceful handling of database exceptions (e.g. duplicate inserts)
* Optimized data access with reusable repository methods

---

## 🧠 Business Logic Examples

* Prevent duplicate vehicle registrations
* Validate vehicle-brand-model relationships
* Ensure insurance pricing exists before registration
* Calculate registration price dynamically based on:

  * engine power
  * engine capacity
  * vehicle age
  * fuel type

---

## 📂 Project Structure

/backend     → ASP.NET Core Web API
/frontend    → Angular application

---

## 🚀 How to Run

### Backend

1. Clone the repository
2. Copy `appsettings.example.json` to `appsettings.json` and fill in your values
3. Set up User Secrets with your connection string, JWT key, and email password
4. Apply database migrations:
```
   dotnet ef database update --context VehicleRegistrationDbContext
   dotnet ef database update --context AuthDbContext
```
5. Navigate to the backend folder and run:

```
   cd backend/Vehicle-Registration-System
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

## Test Data

The database seeds default users on first migration:
- Admin: `admin@test.com` / `Admin123!`
- Manager: `manager@test.com` / `Manager123!`  
- Employee: `employee@test.com` / `Employee123!`

---

## 👤 Author

Danilo Jankovic
Software Developer
GitHub: https://github.com/jankovicdanilo
