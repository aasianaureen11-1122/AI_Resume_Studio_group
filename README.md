# AI Resume Studio 🤖📄

AI Resume Studio is a smart web-based resume builder and analyzer designed to help users create professional resumes and improve existing resumes using AI-powered feedback.

The application allows users to create resumes from scratch by entering their personal information, education, skills, and work experience. Users can also analyze an existing resume to receive a resume score, identify missing skills, and get suggestions for improvement.

The project is developed using **ASP.NET Core MVC**, **C#**, **SQL Server**, and **Entity Framework Core**, with a responsive frontend built using **HTML, CSS, Razor Views, and Bootstrap**. Future versions will integrate the **OpenAI API** to provide intelligent resume analysis, personalized suggestions, and content enhancement.

---

## 🚀 Features

### 📄 Resume Builder

* Create professional resumes from scratch
* Add personal information
* Add educational qualifications
* Add work experience
* Add technical and professional skills
* Add a professional summary
* Select different resume templates

### 🤖 AI Resume Analyzer

* Analyze uploaded resumes
* Evaluate resume quality
* Generate a resume score
* Identify missing or recommended skills
* Provide AI-powered improvement suggestions
* Improve resume content and descriptions

### 🎨 Resume Templates

* Multiple professional resume templates
* Modern and clean designs
* Template-based resume generation

### 📊 Resume Management

* Create and save multiple resumes
* View saved resumes
* Manage resume information
* Generate a professional resume document

---

## 🛠️ Technologies Used

### Frontend

* HTML5
* CSS3
* Bootstrap
* Razor Views

### Backend

* C#
* ASP.NET Core MVC
* Object-Oriented Programming (OOP)

### Database

* Microsoft SQL Server
* Entity Framework Core

### AI Integration

* OpenAI API *(planned / future integration)*

### Development Tools

* Visual Studio
* SQL Server Management Studio (SSMS)
* Git
* GitHub

---

## 🏗️ Project Architecture

The project follows the **MVC (Model-View-Controller)** architecture.

```text
User
  │
  ▼
Razor Views / Frontend
  │
  ▼
Controller
  │
  ▼
Business Logic / Services
  │
  ▼
Entity Framework Core
  │
  ▼
SQL Server Database
```

The application is divided into three main MVC components:

* **Model** – Represents application data and database entities.
* **View** – Provides the user interface using Razor Views, HTML, CSS, and Bootstrap.
* **Controller** – Handles user requests and connects the frontend with business logic and the database.

---

## 🗄️ Database

The project uses SQL Server to store application data.

The main database entities include:

* Users
* Resumes
* Education
* Experience
* Skills
* Templates
* Resume Analysis

The database is connected to the ASP.NET Core application using **Entity Framework Core**.

---

## 📁 Project Structure

A simplified project structure is shown below:

```text
AIResumeStudio/
│
├── Controllers/
│   ├── HomeController.cs
│   ├── ResumeController.cs
│   └── ResumeAnalysisController.cs
│
├── Models/
│   ├── User.cs
│   ├── Resume.cs
│   ├── Education.cs
│   ├── Experience.cs
│   ├── Skill.cs
│   ├── Template.cs
│   └── ResumeAnalysis.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Services/
│   └── ResumeAnalyzerService.cs
│
├── Views/
│   ├── Home/
│   ├── Resume/
│   ├── ResumeAnalysis/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
│
├── appsettings.json
├── Program.cs
└── AIResumeStudio.csproj
```

---

## ⚙️ Installation and Setup

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR-USERNAME/AIResumeStudio.git
```

Move into the project directory:

```bash
cd AIResumeStudio
```

### 2. Open the Project

Open the project in **Visual Studio**.

Make sure the required .NET SDK and ASP.NET Core components are installed.

### 3. Configure SQL Server

Update the connection string in:

```text
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=AIResumeStudioDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Change the SQL Server name according to your local SQL Server installation.

### 4. Create the Database

You can create the database using either:

* SQL Server Management Studio (SSMS)
* Entity Framework Core migrations

For EF Core migrations:

```bash
Add-Migration InitialCreate
Update-Database
```

Or using the .NET CLI:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the Application

Run the project from Visual Studio or use:

```bash
dotnet run
```

Open the local URL shown in the terminal or Visual Studio.

---

## 🔮 Future Improvements

The following features are planned for future versions:

* OpenAI API integration
* Advanced AI resume scoring
* ATS (Applicant Tracking System) optimization
* Job description matching
* AI-generated professional summaries
* AI-generated experience descriptions
* Skill gap analysis
* Personalized career recommendations
* PDF resume generation
* More resume templates
* User authentication and authorization
* Resume sharing through a public link

---

## 🎯 Project Goal

The main goal of AI Resume Studio is to combine **resume creation** and **AI-powered resume analysis** into a single platform.

The system aims to help students, graduates, and job seekers create professional resumes and receive intelligent feedback to improve their chances of successfully applying for jobs.

---

## 👩‍💻 Author

**Your Name**

Data Science Student
Interested in Artificial Intelligence, Data Science, and Software Development.

---

## 📜 License

This project is created for educational and development purposes.

You can add a specific open-source license, such as the MIT License, in the future.

