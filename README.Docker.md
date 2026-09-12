### Building and running your application

To build and start your application along with the MS SQL Server database, run:

```bash
docker compose up --build
```

### Services included:
- **`server`**: ASP.NET Core MVC Stock Management web application (runs on http://localhost:8080)
- **`db`**: Microsoft SQL Server 2022 instance (exposes port 1433)
- **`db-init`**: Automatic script runner that initializes the database schema, tables (`Material`, `User`, `StockTransaction`), and stored procedures from `init-db.sql`.

### Stopping the application

To stop and remove containers and networks:

```bash
docker compose down
```

To also remove stored database volumes:

```bash
docker compose down -v
```