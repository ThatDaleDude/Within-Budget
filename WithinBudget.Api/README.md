# WithinBudget.Api

## Running SQL Server with Docker

To run the required SQL Server instance for this project, create a Docker container using the following command:

```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD={THE_PASSWORD}" -p 1433:1433 --name WithinBudget-SQL -d mcr.microsoft.com/mssql/server:2022-latest
```

This will start a SQL Server 2022 container named `WithinBudget-SQL` with the necessary environment variables and port mapping.

If you don't have the password, and you're just wanting to test this application out, feel free to create your own DB and set the password as required in the application's
default connection string
