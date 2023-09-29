# DotNetAtom
This repository houses an experimental project to implement DotNetNuke in ASP.NET Core.

The goal is to provide a similair experience to the current DotNetNuke platform, but it's not compatible with the current platform.

## Getting started
First, you'll need a DNN9 installation since no database is being created yet.  
To run the project, add a connection string called `SqlServer` and run `DotNetAtom.Platform`.

Example:
```bash
dotnet run --project src/DotNetAtom.Platform /ConnectionStrings:SqlServer="Data Source=localhost;Initial Catalog=dnn;User ID=sa;Password=password;TrustServerCertificate=true"
```

## Structure
The solution is split up in multiple projects:

| Path | Description |
| ---- | ----------- |
| src/DotNetAtom.Abstractions | Abstractions of the core framework. Modules and Themes should reference this project. |
| src/DotNetAtom.Core | The core framework with implementations of the abstractions. |
| src/DotNetAtom.Web | Base project for the web application. |
| src/DotNetAtom.Platform | Implementation of the platform, which includes Xcillion, HTML module and DDRMenu. This might split up in the future. |
| src/DotNetAtom.Repositories.DapperAOT | Implementation of the repositories using DapperAOT. This is used in NativeAOT. |
| src/DotNetAtom.Repositories.DotNetNuke | Experimental implementation of the repositories using the current DotNetNuke platform. |
| src/DotNetAtom.Repositories.EntityFrameworkCore | Implementation of the repositories using EF Core. |

The web platform is using [WebFormsCore](https://github.com/WebFormsCore/WebFormsCore) which is a port of the ASP.NET WebForms framework to ASP.NET Core.  