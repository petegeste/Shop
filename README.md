# Shop

A project to test kubernetes development.

## Structure

**shop-db**

An EF Core project that provides access to a postgres database. This will be hosted in a managed AWS instance,
but for now it can be developed in Docker.

**shop-api**

Backend "microservice" that provides access to managed database.

**shop-webapp**

Frontend "microservice" that hosts the web UI.

## Development
You can run this project in Visual Studio using the docker-compose project. This will launch a postgres container
and the other services in debug.
