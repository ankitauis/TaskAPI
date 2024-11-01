# TaskAPI

# Project Setup Instructions

This .NET Core project is integrated with MSSQL and requires Visual Studio 2022. Follow the steps below to configure and run the application.

## Prerequisites

1. **Download and Install Visual Studio 2022**  
   Ensure compatibility by installing Visual Studio 2022: [Download Visual Studio](https://visualstudio.microsoft.com/vs/).

2. **Install MSSQL**  
   Download and install Microsoft SQL Server to manage the project database: [Download MSSQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).

## Setup Steps

1. **Clone the Repository**
Open the solution (.sln) in Visual Studio 2022.

2. **Configure Database Connection**  
In `appsettings.json`, update `DefaultConnection` with your server and database names:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=<your-server-name>;Database=<your-database-name>;Trusted_Connection=True;"
}
```

3. **Run Database Migrations**
In Visual Studio, open the NuGet Package Manager Console (Tools -> NuGet Package Manager -> Package Manager Console) and run:
  update-database


