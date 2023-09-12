#Setup


## Prerequisites:
- Docker installed on your system.
- .NET Core SDK installed.
- Entity Framework Core CLI tools installed.

## Setup Instructions:

### 1. Pull the MariaDB Docker Image:
```docker pull mariadb```

### 2. Run the MariaDB Container:
```docker run --detach --name todo-db --env MARIADB_ROOT_PASSWORD=password --env MARIADB_DATABASE=todo-db -p 3306:3306 mariadb:latest```

### 4. Install EF Core Provider for MariaDB:
```dotnet add package Pomelo.EntityFrameworkCore.MySql```
### 5. Scaffold the DbContext from MariaDB:
```dotnet ef dbcontext scaffold "server=localhost;port=3306;user=root;password=password;database=todo-db" Pomelo.EntityFrameworkCore.MySql -o Models```
### 6. Enable Migrations for Your Project:
```dotnet ef migrations add InitialCreate```
### 7. Update or Create the Database Based on Migrations:
```dotnet ef database update```
### 8. Run Your .NET Core Application:
```dotnet run```