# StockManagment
This Project is a simple stock management system that allows users to manage their inventory, track stock levels.
It is built using MVC, .NET 9.0 and C# 13.0, ensuring compatibility with the latest features.

The project includes a simple UI to interact with the functions as well as Authentication and Authorization features to
ensure that only authorized users can access the system using JWT and cookies

Sql Server database is used to store the inventory data, providing a robust and reliable backend for the application.

SQL Queries are used to interact with the database, allowing users to perform CRUD (Create, Read, Update, Delete) operations on their stock items.

Table creation scripts are included in the project to set up the necessary database tables for storing stock information ( in SQL Scripts text file).


Also includes Logging for Error and Information messages to help with debugging and monitoring the application.


Notes:
- Ensure to update the connection string in `appsettings.json` with your actual database connection details.
- Replace the `TokenKey` and `PasswordKey` with secure keys in a production environment
- Make sure to run the SQL scripts provided in the project to create the necessary tables before running the application.
- Usage of Secrets Manager or Azure key vault or Enviroment Variables is recommended for storing sensitive information like connection strings and keys in a production environment.
- Logs is being created in Logs Folder in the project directory, ensure that the application has write permissions to this folder.


Assumptions:
- I've assumed that a normal user can Login,Create Material and Tranasction as well as editing or deleting them, which isn't right but will be changed based on the business needs.
- I've also added highlighted the Materials that are close to the minimum threshold with a different color to grab the attention of the user.

Packages Used:
-Nlog
-Microsoft.AspNetCore.Authentication.JwtBearer
-dapper
