# RareForum

![Made in 7 Days](https://img.shields.io/badge/made%20in-7%20days-blue) ![Status](https://img.shields.io/badge/status-no%20longer%20maintained-red)

**Frontend:** ![Razor](https://img.shields.io/badge/razor-563d7c?logo=dotnet&logoColor=white) ![Bootstrap](https://img.shields.io/badge/bootstrap-7952B3?logo=bootstrap&logoColor=white) ![Markdown](https://img.shields.io/badge/markdown-000000?logo=markdown&logoColor=white)  
**Backend:** ![.NET Core 8](https://img.shields.io/badge/.NET%20Core-512BD4?logo=dotnet&logoColor=white) ![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white) ![MSSQL](https://img.shields.io/badge/MSSQL-CC2927?logo=microsoftsqlserver&logoColor=white) ![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?logo=dotnet&logoColor=white)

---

## 📑 Table of Contents
- [Summary (Short Version)](#summary-short-version)
- [Disposition (Full Project Description)](#disposition-full-project-description)
    - [System Features](#system-features)
    - [User Management](#user-management)
    - [Access Rights](#access-rights)
    - [Security](#security)
    - [Technologies](#technologies)
- [Summary](#summary)

---

## Summary (Short Version)

RareForum is a forum system with categories, messages, comments, Markdown support, and basic user management.  
Built with .NET Core 8, Razor, Bootstrap, and MSSQL (Entity Framework Core).

⚠️ Security is simplified for demonstration purposes and **not production-ready**.

[🔼 Back to top](#RareForum)

---

## Disposition (Full Project Description)

The project was developed as a technical challenge to test and improve my skills.  
Basic forum features are relatively straightforward to implement, but the system allows for complex extensions.  
The forum was created with core functionality that includes the creation of categories, messages, and comments, as well as the ability to edit and delete them.  
In addition, extended functionality has been implemented such as user management, ownership and editing rights for categories, messages, and comments, as well as user registration, login, profile management, and the ability to create categories inside other categories.

[🔼 Back to top](#RareForum)

### System Features
The system allows users to create categories, which can contain both subcategories and messages.  
Users can also create messages within a category and add comments to them.  
All created elements can be edited or deleted by their owners.

This structure ensures a flexible and organized way of conducting discussions, where content can be expanded and managed as needed.  
Additionally, messages and comments support the use of **Markdown**, allowing users to format their posts with, for example, headings, lists, and emphasized text.

[🔼 Back to top](#RareForum)

### User Management
To create, edit, or delete categories, messages, and comments, the user must be logged in.  
Each user has a profile where they can provide information such as first name, last name, phone number, birthday, and location.  
These details can be edited as needed, allowing users to manage their own data within the system.

[🔼 Back to top](#RareForum)

### Access Rights
Only the owner of a category, message, or comment has the rights to edit or delete their own content.  
Other users only have access to read and interact with publicly available content.

This ensures that users have full control over their contributions while creating a secure and organized platform for interaction.

[🔼 Back to top](#RareForum)

### Security
The system requires users to be logged in to create, edit, or delete content, preventing unauthorized manipulation.  
Since building proper and secure user management and authentication is a large task, this project uses a very simplified implementation, one that I would **never** use in production.

If this project were to be used in production, the entire security system would need to be redesigned.  
The first step would be to remove plain-text passwords from the database and implement a more secure authentication approach.

[🔼 Back to top](#RareForum)

### Technologies
The application is developed using a set of technologies that cover frontend, backend, and database integration.

**Frontend:** Razor for dynamic generation of HTML, Bootstrap and Bootstrap Icons for responsive and user-friendly design, EasyMDE Markdown editor for user-friendly text formatting, and Markdig combined with Westwind.AspNetCore.Markdown for rendering Markdown.

**Backend:** C# with .NET Core 8 for a robust and scalable architecture.

**Database:** Microsoft SQL Server (MSSQL) with Entity Framework Core, designed with a *code-first* approach and migrations for version control.

[🔼 Back to top](#RareForum)

---

## Summary
The combination of these technologies ensures an efficient, scalable, and maintainable forum system.

[🔼 Back to top](#RareForum)