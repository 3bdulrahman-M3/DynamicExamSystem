
# Exam Management System

This project is developed as part of an internship at Atos and serves as a comprehensive platform for managing exams. The system includes features for creating, updating, and deleting exams, managing questions, evaluating results, and more. The backend is built with .NET 8, while the frontend is powered by Angular 18. SignalR is integrated for real-time notifications.

## Features

### Admin Features

-   **Create Exam**: Admins can create new exams and assign them to specific subjects.
-   **Manage Questions**: Add, update, and delete questions for exams.
-   **View Reports**: Generate and view reports of exam results.
-   **Real-time Notifications**: Receive notifications when students submit exams.

### Student Features

-   **Take Exam**: Students can participate in assigned exams.
-   **Submit Answers**: Submit answers for evaluation.
-   **View Results**: Access their exam results.

## Technologies Used

### Frontend

-   Angular 18
-   Tailwind CSS
-   SignalR for real-time communication

### Backend

-   .NET 8
-   Entity Framework Core
-   AutoMapper
-   SignalR for real-time communication

### Database

-   SQL Server

### Tools

-   Visual Studio Code
-   Visual Studio 2022
-   Postman

## Setup Instructions

### Prerequisites

1.  .NET 8 SDK
2.  Node.js and npm
3.  SQL Server
4.  Angular CLI

### Backend Setup

1.  Clone the repository.
2.  Navigate to the backend directory.
3.  Run `dotnet restore` to install dependencies.
4.  Update the `appsettings.json` file with your database connection string.
5.  Run database migrations using `dotnet ef database update`.
6.  Start the backend server with `dotnet run`.

### Frontend Setup

1.  Navigate to the frontend directory.
2.  Run `npm install` to install dependencies.
3.  Start the Angular development server with `ng serve`.

### SignalR Setup

Ensure the backend SignalR hub is running and correctly configured in the Angular SignalR service.

## Usage

### Admin Panel

1.  Log in as an admin to access the dashboard.
2.  Navigate to the "Exams" section to manage exams.
3.  Use the "Reports" section to view exam submissions and statistics for all students.

### Student Portal

1.  Log in as a student to access assigned exams.
2.  Complete and submit exams within the given timeframe.
3.  View results in the "My Results" section.

## Contribution

This project is developed as part of an internship at Atos. Contributions are currently not open to external collaborators.

## License

This project is developed under Atos as part of an internship program and is not open-sourced.

## Acknowledgments

Special thanks to the Atos team for their guidance and support throughout the development of this project.
