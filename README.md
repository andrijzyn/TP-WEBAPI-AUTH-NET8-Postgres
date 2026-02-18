## Intro

This repository stores my notes while learning ASP.NET and setting up my local configuration.  
All code comes from public sources and AI.

---

## For the future

* `./appsettings.json` must not be committed to a public repo.  
It must contain a connection string like:

      ConnectionStrings:DefaultConnection
      Host=127.0.0.1;Port=5432;Database=todo_db;Username=todo_user;Password=...

* If you want change DBMS, all works through middleware:

> change the EF Core provider package in web.csproj (for example from Npgsql.EntityFrameworkCore.PostgreSQL to Microsoft.EntityFrameworkCore.SqlServer),
> 
> update UseNpgsql to the corresponding UseSqlServer (or other) in Program.cs,
> 
> adjust the connection string in appsettings.json to match the new database engine.

---

## Notes

`[FromServices]` tells Minimal API to resolve `AppDbContext` from DI, and `TodoItem` comes from the request body.

---

## 1. Environment setup

Required packages:

    dotnet-sdk-8.0
    aspnet-runtime-8.0
    aspnet-targeting-pack-8.0
    postgresql

PostgreSQL init on Arch:

    sudo -iu postgres
    initdb -D /var/lib/postgres/data
    exit

    sudo systemctl start postgresql
    sudo systemctl enable postgresql

---

## 2. Database setup

Create user and database:

    sudo -iu postgres
    psql
    CREATE USER todo_user WITH PASSWORD 'strong_password';
    CREATE DATABASE todo_db OWNER todo_user;
    GRANT ALL PRIVILEGES ON DATABASE todo_db TO todo_user;
    \q
    exit

EF Core tools and schema:

    dotnet tool install --global dotnet-ef
    dotnet ef migrations add InitialCreate
    dotnet ef database update

---

## 3. HTTPS dev certificate for Kestrel

    dotnet dev-certs https --trust

---

## 4. First run

    dotnet restore
    dotnet run

Swagger UI to test endpoints:

    https://localhost:7077/swagger/index.html
