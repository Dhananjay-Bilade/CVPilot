# CVPilot 🚀

## Your Career. Ready for Takeoff.

CVPilot is a **.NET MAUI mobile application** for creating, managing,
analyzing, previewing, and exporting professional resumes.

It combines a **Resume Builder, ATS Checker, Job Description Matcher,
and PDF Exporter** in one application.

## ✨ Features

### Resume Builder

Create complete resumes with: - Personal Details - Professional Summary
/ Objective - Education - Experience - Skills - Projects -
Certifications - Languages - References - Achievements

### Resume Management

-   Create multiple resumes
-   Resume categories: Professional, Fresher, Student, Academic
-   Edit existing resumes
-   Delete resumes
-   Track completion percentage
-   Store created and updated dates
-   Preview resumes before export

### 📑 Professional PDF Export

CVPilot uses **Syncfusion.Pdf.NET** to generate professional A4 resumes.

PDF features include: - A4 page layout - Professional margins and
typography - Multi-page PDF generation - Automatic page breaks -
Structured resume sections - Clickable LinkedIn, GitHub, portfolio and
other URLs - Android system file picker for saving PDFs

### 🤖 ATS Checker

The ATS analyzer evaluates important resume areas using
application-defined weights:

  Category                 Weight
  --------------------- ---------
  Contact Information          15
  Resume Sections              20
  Skills                       20
  Experience                   15
  Education                    10
  Keywords                     10
  Formatting                   10
  **Total**               **100**

The ATS score is intended as guidance and does not guarantee the result
of any external Applicant Tracking System.

### 🎯 Job Description Matcher

Compares resume content with a job description and identifies: - Matched
keywords - Missing keywords - Keyword match percentage - Important
job-related terms

### 💾 Local SQLite Database

Resume data is stored locally on the device using SQLite.

Main database entities include: - Resume - PersonalDetails - Education -
Experience - Skill - Project - Objective - Certification - Language -
Reference - Achievement

### ⚙️ Settings

Users can configure: - Light theme - Dark theme - System Default theme -
Default resume category

### 📱 Android Support

The primary tested platform is Android. PDF files are saved through the
Android Storage Access Framework so users can select the destination
using the system file picker.

## 🏗️ Architecture

``` text
CVPilot/
│
├── Models/
├── Database/
├── Services/
├── ViewModels/
├── Views/
│   ├── Home/
│   ├── Resume/
│   ├── Sections/
│   ├── Settings/
│   └── ATSChecker/
│
├── Platforms/
│   └── Android/
├── Resources/
├── Properties/
│
├── App.xaml
├── App.xaml.cs
├── AppShell.xaml
├── AppShell.xaml.cs
├── MauiProgram.cs
└── CVPilot.csproj
```

## 🧩 Core Services

### DatabaseService

Handles SQLite initialization and CRUD operations for resumes and their
sections.

### ResumeDataService

Handles resume-related data operations used throughout the application.

### PdfService

Creates the professional A4 PDF, manages page breaks, adds clickable
links, and handles PDF export.

### ATSAnalyzerService

Analyzes resume structure, contact information, sections, skills,
experience, education, keywords, and formatting.

### JobDescriptionMatcherService

Compares resume content against job descriptions and calculates keyword
matching information.

## 🧭 Application Flow

``` text
Create Resume
      ↓
Enter Resume Information
      ↓
Save to SQLite
      ↓
Complete Resume Sections
      ↓
Preview Resume
      ↓
ATS Analysis
      ↓
Job Description Matching
      ↓
Export PDF
      ↓
Save PDF on Device
```

## 🛠️ Technology Stack

  Technology              Purpose
  ----------------------- ---------------------------
  C#                      Application programming
  .NET 10                 Framework
  .NET MAUI               Cross-platform UI
  XAML                    User interface
  SQLite                  Local data storage
  sqlite-net-pcl          SQLite access
  Syncfusion.Pdf.NET      PDF generation
  CommunityToolkit.Maui   MAUI utilities/components
  Visual Studio 2026      Development

## 📦 Main NuGet Packages

``` xml
CommunityToolkit.Maui
CommunityToolkit.Maui.Core
Microsoft.Maui.Controls
sqlite-net-pcl
Syncfusion.Pdf.NET
```

## 💉 Dependency Injection

CVPilot uses .NET dependency injection to register application services,
including:

``` text
DatabaseService
ResumeDataService
ATSAnalyzerService
JobDescriptionMatcherService
```

Pages obtain required services through the MAUI service provider.

## 🗃️ Data Management

The application uses a local SQLite database located in the
application's local data directory.

This allows users to create and manage resumes without requiring a
remote backend for the core resume-building workflow.

## 🔐 License & Security

CVPilot uses **Syncfusion.Pdf.NET** under a Syncfusion Community
License.

**Never commit a real license key, password, API key, or other secret
credential to a public GitHub repository.**

For a public repository, use a placeholder such as:

``` csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
    "YOUR_SYNCFUSION_LICENSE_KEY");
```

Keep the actual license key private and configure it locally.

## 🚀 Getting Started

### Requirements

-   Visual Studio 2026
-   .NET 10 / .NET MAUI workload
-   Android SDK
-   Android emulator or Android device
-   Required NuGet packages
-   Valid Syncfusion license configuration for PDF functionality

### Setup

1.  Clone or download the repository.
2.  Open `CVPilot.csproj` in Visual Studio.
3.  Restore NuGet packages.
4.  Configure the Syncfusion license securely.
5.  Select an Android emulator or connected device.
6.  Build the project.
7.  Run CVPilot.

## 📂 Important Files

  -----------------------------------------------------------------------
  File                                Purpose
  ----------------------------------- -----------------------------------
  `App.xaml`                          Global application resources

  `AppShell.xaml`                     Shell configuration

  `AppShell.xaml.cs`                  Navigation route registration

  `MauiProgram.cs`                    Application configuration and
                                      dependency injection

  `CVPilot.csproj`                    Project and package configuration

  `DatabaseService.cs`                SQLite database operations

  `PdfService.cs`                     PDF generation and export

  `ATSAnalyzerService.cs`             ATS analysis

  `JobDescriptionMatcherService.cs`   Job keyword matching
  -----------------------------------------------------------------------

## 🧪 Tested Workflow

The main application workflow has been tested, including: - Creating
resumes - Saving resume data - Editing sections - Viewing saved
resumes - Previewing resumes - Multi-page PDF generation - PDF export
and Android file saving - Clickable PDF links - ATS analysis - Job
description matching - Theme settings - Default resume category
settings - Syncfusion Community License configuration

## 🎯 Project Objective

CVPilot was developed as a practical demonstration of:

-   .NET MAUI development
-   C# programming
-   XAML UI design
-   SQLite database integration
-   CRUD operations
-   Dependency Injection
-   Shell navigation
-   PDF generation
-   Android file handling
-   Resume management
-   ATS-oriented analysis
-   Keyword matching
-   Application preferences

## 🔮 Future Enhancements

Possible future improvements include: - Additional resume templates -
More ATS rules - AI-powered resume suggestions - Cloud synchronization -
Resume sharing - Advanced job matching - More export formats - Advanced
analytics - More customization options

## 📌 Project Status

**Completed ✅**

CVPilot provides the complete workflow:

**Create → Manage → Improve → Analyze → Preview → Export**

## 📄 Third-Party Licenses

Third-party libraries and components used by this project are governed
by their respective licenses and terms.

Syncfusion components are subject to the applicable Syncfusion Community
License terms.

------------------------------------------------------------------------

## 🚀 CVPilot

**Your Career. Ready for Takeoff.**

Build your resume. Improve your content. Check your ATS readiness. Match
it with a job description. Export a professional PDF.
