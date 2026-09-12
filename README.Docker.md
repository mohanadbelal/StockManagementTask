# Docker Deployment Guide - Stock Management System

This directory contains full Docker containerization support for the Stock Management System application and Microsoft SQL Server 2022.

---

## Container Services Overview

| Service | Image | Description | Ports |
| :--- | :--- | :--- | :--- |
| **`server`** | `assignmenttask-server` | ASP.NET Core MVC application | `8080`, `9000` |
| **`db`** | `mcr.microsoft.com/mssql/server:2022-latest` | Microsoft SQL Server 2022 | `1433` |
| **`db-init`** | `mcr.microsoft.com/mssql/server:2022-latest` | Ephemeral service running `init-db.sql` | N/A |

---

## Quick Start Commands

### Start All Services

```bash
docker compose up --build -d
```

### View Application Logs

```bash
# View web server logs
docker compose logs -f server

# View database logs
docker compose logs -f db
```

### Stop All Services

```bash
# Stop containers (retaining database data)
docker compose down

# Stop containers and wipe database volumes
docker compose down -v
```

---

## Environment & Volume Configuration

- **Environment & App Configuration (`.env` / `appsettings.json`)**: Secrets and keys such as `MSSQL_SA_PASSWORD`, `TOKEN_KEY`, and `PASSWORD_KEY` can be configured via `.env` (copied from `.env.example`). Security tokens (`TokenKey` and `PasswordKey`) are read directly from `appsettings.json` or overridden via environment variables.
- **Application Logs (`./Logs`)**: Container log output at `/app/Logs` is mounted to the host project directory `./Logs`.
- **Database Volume (`mssql-data`)**: SQL Server data files persist in named volume `mssql-data`.