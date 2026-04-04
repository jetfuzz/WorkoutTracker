# Workout Tracker
ASP.NET MVC web application for logging workouts, visualizing exercise progression, and tracking personal records over time.

![Dashboard](/screenshots/Screenshot%202026-04-03%20190022.png)

🔗 **[Live Demo](https://workouttracker-app-cza4gkcfbhb3ahec.canadacentral-01.azurewebsites.net/)**

## Features
* User authentication with Identity Framework
* Create and log workouts with exercises, sets, reps, and weight
* View workout history with volume and set counts per workout
* Track exercise progression over time with filterable charts
* Personal record tracking

## Technologies Used
* **Backend:** ASP.NET Core MVC, Entity Framework Core, ASP.NET Core Identity, C#
* **Frontend:** Bootstrap 5, Chart.js, JavaScript, HTML/CSS
* **Deployment:** Azure App Service, Azure SQL Database

## Installation
* Ensure [.NET 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) is installed
* `git clone https://github.com/jetfuzz/WorkoutTracker.git`
* `cd WorkoutTracker`
* `dotnet restore`
* `dotnet build`
* Configure your SQL Server connection string in `appsettings.json` or via user secrets
* `dotnet run`