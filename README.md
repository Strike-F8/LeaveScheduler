# LeaveScheduler
On first run of the application, the SeedData class will populate the database. If there are employees already in the database, it will not populate.

## Dependencies
This application requires a local SQL server.
The easiest way to satisfy this is to install SQL Server Express 2019 and start it using SQL Server Management Studio.

The application automatically migrates the SQL database at runtime if needed so no further configuration is necessary.
Simply run the program.
