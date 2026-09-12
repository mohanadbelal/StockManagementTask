# Stock Management System

A stock management web application built with **ASP.NET Core MVC (.NET 10)**, **C#**, **Dapper**, and **Microsoft SQL Server**. It provides inventory tracking, stock transaction management, automated low-stock indicators, JWT-based authentication/authorization, and full Docker containerization.

---

## Key Features

- **Inventory & Stock Tracking**: Perform CRUD operations on materials (`Material` table) and monitor current stock versus minimum required stock thresholds.
- **Stock Transactions**: Record stock-in and stock-out transactions (`StockTransaction` table) with stored procedures dynamically updating inventory balances.
- **Authentication & Authorization**: User registration and login using PBKDF2 password hashing, JWT tokens stored in HTTP-only cookies, and role-based access control.
- **Automated Database Setup**: Includes SQL scripts (`init-db.sql`) to automatically provision database schemas, tables, constraints, and stored procedures.
- **Logging**: Integrated **NLog** provider writing logs both to the console and to structured log files in `./Logs`.
- **Docker & Docker Compose Integration**: Fully containerized environment with web server, MS SQL Server 2022 instance, and auto-initialization service.

---

## Quick Start with Docker Compose (Recommended)

### 1. Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.

### 2. Configure Environment Variables
Copy `.env.example` to `.env`:

```bash
cp .env.example .env
```

The `.env` file allows customizing database credentials and application security keys:

```env
# Database Configuration
MSSQL_SA_PASSWORD=YourStrong@Password123
DB_SERVER=db
DB_PORT=1433
DB_NAME=StockManagementDb
DB_USER=sa

# Application Security Keys (Passed to app settings / environment variables)
TOKEN_KEY=testTokenKey-ThisWillbeReplacedWithTheActualKeyTobelongenoughfor512Bit
PASSWORD_KEY=PasswordKey-ThisWillbeReplacedWithTheActualKey
```

### 3. Build & Run Containers
Run Docker Compose to build the application and start SQL Server + DB initialization:

```bash
docker compose up --build -d
```

### 4. Access the Application
- Web UI: **[http://localhost:9000](http://localhost:9000)** or **[http://localhost:8080](http://localhost:8080)**
- MS SQL Server: **`localhost:1433`** (`sa` / `YourStrong@Password123`)

---

## Local Development (Without Docker)

1. Update `appsettings.json` with your local SQL Server instance connection string and security keys:
   ```json
   {
     "ConnectionStrings": {
       "DBConnection": "Server=.\\SQLEXPRESS;Database=StockManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "TokenKey": "YourSecretJwtTokenKeyHere...",
     "PasswordKey": "YourPasswordSaltKeyHere..."
   }
   ```
   *Note: Security tokens (`TokenKey` and `PasswordKey`) are read directly from `appsettings.json` (or environment variables).*
2. Run `init-db.sql` or `SQL Scripts.txt` on your SQL Server instance to create the database tables and stored procedures.
3. Launch the application:
   ```bash
   dotnet run
   ```

---

## Architecture & Technology Stack

- **Framework**: ASP.NET Core 10.0 (MVC)
- **Database**: Microsoft SQL Server 2022
- **ORM / Data Access**: Dapper
- **Authentication**: JWT Bearer & Cookie-based Session
- **Logging**: NLog (Console & File targets mapped to `./Logs`)
- **Containerization**: Docker, Docker Compose

---

## Stopping & Cleaning Containers

- Stop containers: `docker compose down`
- Stop containers and delete database volume: `docker compose down -v`
