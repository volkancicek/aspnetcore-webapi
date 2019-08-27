# ASP.NET Core (v2.2) Web API project
An ASP.NET Core (v2.2) Web API project which consists basic endpoints to store and manage appointments for a car service.

# Build & Run

* Visual Studio

Open the solution file <code>AspNetCore.WebApi.sln</code> and build/run.

* Visual Studio Code

Open the <code>src</code> folder and <code>F5</code> to build/run.


# Swagger UI
Basic information about the API endpoints can be found at the Swagger UI which is available with default url.
<http://localhost:62769/index.html>

Enpoints:
- GET `/api/appointment/{id}` - Gets single appointment.
- DELETE `/api/appointment/{id}` - Removes appointment with given id.
- PATCH `/api/appointment/{id}` - Updates appointment status
- POST `/api/appointment/` - Creates a new appointment.
- GET `/api/appointment/bydate` - Gets appointments within given period.

# Contact
volkancicek@outlook.com
 
