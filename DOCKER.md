# Docker Setup

This project runs with two containers:

- `ecommerce-web`: the ASP.NET Core MVC app from `EcommerceSystem.PL`
- `ecommerce-db`: SQL Server 2022 with a persistent Docker volume

Start everything:

```powershell
docker compose up --build
```

Open the app:

```text
http://localhost:8080
```

The app keeps the existing local SQL Server connection string in `appsettings.json`. Docker Compose overrides it with:

```text
Server=ecommerce-db,1433;Database=ECommerceSystemV02;User Id=sa;...
```

The database schema and seed data are created automatically in Docker because Compose sets `ApplyMigrationsOnStartup=true`.

Docker also creates default Identity accounts:

| Email | Password | Role |
| --- | --- | --- |
| `admin@gmail.com` | `Admin123!` | `Admin` |
| `user@gmail.com` | `User123!` | `User` |

You can change them before startup with environment variables:

```powershell
$env:DEFAULT_ADMIN_EMAIL = "admin@example.com"
$env:DEFAULT_ADMIN_PASSWORD = "Admin123!"
$env:DEFAULT_USER_EMAIL = "user@example.com"
$env:DEFAULT_USER_PASSWORD = "User123!"
docker compose up --build
```

To use a different SQL Server password:

```powershell
$env:MSSQL_SA_PASSWORD = "Another_Strong123!"
docker compose up --build
```

Stop containers:

```powershell
docker compose down
```

Remove the database volume and start with a clean database:

```powershell
docker compose down -v
```
