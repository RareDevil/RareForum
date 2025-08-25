# RareForum

The project was developed as a technical challenge to test and improve my skills.  
Basic forum features are relatively straightforward to implement, but the system allows for complex extensions.  
The forum was created with core functionality that includes the creation of categories, messages, and comments, as well as the ability to edit and delete them.  
In addition, extended functionality has been implemented such as user management, ownership and editing rights for categories, messages, and comments, as well as user registration, login, profile management, and the ability to create categories inside other categories.

---

## System Features

The system allows users to create categories, which can contain both subcategories and messages.  
Users can also create messages within a category and add comments to them.  
All created elements can be edited or deleted by their owners.

This structure ensures a flexible and organized way of conducting discussions, where content can be expanded and managed as needed.  
Additionally, messages and comments support the use of **Markdown**, allowing users to format their posts with, for example, headings, lists, and emphasized text.

---

## User Management

To create, edit, or delete categories, messages, and comments, the user must be logged in.  
Each user has a profile where they can provide information such as first name, last name, phone number, birthday, and location.  
These details can be edited as needed, allowing users to manage their own data within the system.

---

## Access Rights

Only the owner of a category, message, or comment has the rights to edit or delete their own content.  
Other users only have access to read and interact with publicly available content.

This ensures that users have full control over their contributions while creating a secure and organized platform for interaction.

---

## Security

The system requires users to be logged in to create, edit, or delete content, preventing unauthorized manipulation.  
Since building proper and secure user management and authentication is a large task, this project uses a very simplified implementation, one that I would **never** use in production.

If this project were to be used in production, the entire security system would need to be redesigned.  
The first step would be to remove plain-text passwords from the database and implement a more secure authentication approach.

---

## Technologies

The application is developed using a set of technologies that cover frontend, backend, and database integration.

**Frontend:**
- Razor for dynamic generation of HTML
- Bootstrap and Bootstrap Icons for responsive and user-friendly design
- EasyMDE Markdown editor for user-friendly text formatting
- Markdig combined with Westwind.AspNetCore.Markdown for rendering Markdown

**Backend:**
- C# with .NET Core 8 for a robust and scalable architecture

**Database:**
- Microsoft SQL Server (MSSQL)
- Database interaction handled via Entity Framework Core
- Designed with a *code-first* approach, where the data model is defined in code and the database is created and updated via migrations

This approach ensures a flexible and version-controlled development process, where changes to the data model can be implemented easily without manual database management.

---

## Summary

The combination of these technologies ensures an efficient, scalable, and maintainable forum system.
