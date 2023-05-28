# KnowYourSales Backend

This is the backend application for the KnowYourSales website. The application is built using .NET 6 and utilizes PostgreSQL as the database. The project leverages technologies such as Entity Framework and Dapper for data access.

# About KnowYourSales

KnowYourSales is a RESTful web application designed to help users find products on sale in specific cities. Users can register on the website to receive notifications from specific stores when new products are published. The application allows stores to register and manage multiple shops, while users can also create accounts to access personalized features.

# Technologies Used
The backend application utilizes the following technologies:

- .NET 6: The latest version of the .NET framework for building scalable web applications.
 - PostgreSQL: A robust and open-source relational database management system.
- Entity Framework: A modern object-relational mapper (ORM) used for database interaction and management.
- Dapper: A simple and efficient micro-ORM used for database access and querying.

# Getting Started

To set up and run the KnowYourSales backend application, follow these steps:
1. Clone the repository:
    ```sh
    git clone https://github.com/Rad1c/KnowYourSalesBackend
    cd KnowYourSalesBackend
    ```
2. Navigate to the project directory:
    ```sh
    cd KnowYourSalesBackend
    ```
3. Install the required dependencies using a package manager such as NuGet:
    ```sh
    dotnet restore
    ```
4. Configure the database connection string in the **appsettings.json** file to connect to your PostgreSQL database.
5. Start the application:
    ```sh
    dotnet run
    ```
