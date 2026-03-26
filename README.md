# Vehicle Registration System

Full-stack web application for managing vehicle registrations, clients, insurance policies, and administrative workflows.

Designed as an enterprise-style system with clearly separated backend and frontend layers, focusing on clean architecture, scalability, and real-world backend practices.

---

## 🏗 Architecture

The application follows a layered architecture:

* Presentation Layer (Angular Frontend)
* API Layer (ASP.NET Core Web API)
* Service Layer (Business Logic)
* Repository Layer (Data Access)
* Database Layer (SQL Server)

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
* Advanced validation rules (cross-entity checks such as vehicle–brand–type consistency)
* Concurrency-safe operations using database constraints
* Unique constraints:

  * One registration per vehicle
  * Unique license plate enforcement
* Graceful handling of database exceptions (e.g. duplicate inserts)
* Optimized data access with reusable repository methods

---

## 🧠 Business Logic Examples

* Prevent duplicate vehicle registrations
* Validate vehicle–brand–model relationships
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

* Full-stack development skills
* Backend system design with clean architecture
* Repository pattern and abstraction layers
* Validation and business rule enforcement
* Secure authentication with JWT
* Database modeling and integrity constraints
* Handling real-world backend challenges (concurrency, data consistency)

---

## 👤 Author

Danilo Jankovic
Software Developer
GitHub: https://github.com/jankovicdanilo
