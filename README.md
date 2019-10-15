# ASP.NET Core (v2.2) Web API project
An ASP.NET Core (v2.2) Web API project which consists basic endpoints to store and manage appointments.

# Build & Run

* With Visual Studio

Open the solution file <code>AspNetCore.WebApi.sln</code> and build/run.

* With Command

Install .Net Core 2.2

<https://dotnet.microsoft.com/download>

Check installed dotnet core version with:

`dotnet --version`

Open the folder `/Aspnet.Core.Web.Api` and run the following commands:

`dotnet build`

`dotnet run`

Application will be running at:
<http://localhost:<DEFAULT_PORT>

* With Docker

Install Docker 

<https://docs.docker.com/install/>

Open the folder `/Aspnet.Core.Web.Api` and run the following commands:

`docker image build -t netcoreapi:1.0 .`

`docker run -d -p 8080:80 --name myapp netcoreapi:1.0`


# Swagger UI
Basic information about the API endpoints can be found at the Swagger UI which is available with default url.
<http://localhost:8080/index.html>

Enpoints:
- GET `/api/appointment/{id}` - Gets single appointment.
- DELETE `/api/appointment/{id}` - Removes appointment with given id.
- PATCH `/api/appointment/{id}` - Updates appointment status
- POST `/api/appointment/` - Creates a new appointment.
- GET `/api/appointment/bydate` - Gets appointments within given period.

# Contact
volkancicek@outlook.com
 
