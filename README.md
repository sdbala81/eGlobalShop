### Global eShop

### Docker Compose

If you happen to use Docker Desktop, you can use Docker Compose to run the services. Docker Compose is a tool for
defining and running multi-container Docker applications.

Go to the root directory (eGlobalShop folder) and run the following command to run all the services using docker compose
This command will build the images and start the containers in detached mode.

    docker-compose -f docker-compose.yaml up -d --build -d

To prevent Docker Compose from using the cache and force a clean rebuild and run, use the following command with the
--no-cache option:

    docker compose up --build --no-cache -d

Explanation:

    up: Combines build and start in one command.

    --build: Ensures images are (re)built before starting containers.

    --no-cache: Prevents Docker from using cached layers during the build.

    -d: Runs containers in detached mode.

This is the cleanest way to ensure that every build is fresh with no caching.

Use the following command to stop the services:

```
docker-compose down
```

### Podman Compose

If you happen to use Podman Desktop, you can use Podman Compose to run the services. Podman Compose is a tool that
allows you to define and run multi-container applications using Podman.

Go to the root directory (eGlobalShop folder) and run the following command to run all the services using podman compose

podman requires you use two different commands to build and run the containers. The first command builds the images and
the second command runs the containers.

    podman-compose -f podman-compose.yaml build

    podman-compose -f podman-compose.yaml up -d

Use the following command to stop the services:

    podman-compose down

Once you use any of the containerization methods above, you should be able to see the images and the running containers
in the respective tools i.e. Docker Desktop or Podman Desktop.

The following containers should be running

- postgres-db
- pgadmin
- customer-service
- order-service
- payment-service

Please note that there is single database container which contains the database for all the services. The databases for
the service needs not to be created separately. The databases are created automatically when the services start running.
However the data of the databases will be lost when the containers are stopped. If you want to persist the data, you
need to create a volume for the database container.

### How to connect to the database from the host machine

To connect to pgAdmin from the host machine, follow these steps:

Start the Podman Compose Services. Ensure all services, including pgadmin, are running.

Access pgAdmin via Browser
Open your web browser and navigate to http://localhost:5050. This is the port mapping defined in the podman-compose.yaml
file (5050:80).

Log in to pgAdmin
Use the credentials specified in the pgadmin service environment variables:

```
Email: admin@eGlobalShop.com
Password: postgres

```

* Add a New Server in pgAdmin
* After logging in,Click on "Add New Server".
* In the "General" tab, provide a name for the server (e.g., PostgresDB).
* In the "Connection" tab, fill in the following details.

```
    Host: postgres-db (the service name defined in podman-compose.yaml)
    Port: 5432
    Username: postgres
    Password: postgres
```

* Click "Save" to connect.
* You should now be able to manage your PostgreSQL database using pgAdmin.

### Local Development and Running the services in Development machines without Docker or Podman support using IDE or Command line.

#### Runing the services using *ONLY* Docker or Podman (without Docker Compose or Podman Compose)

From the respective service folders run the following commands

##### If you are using Podman and Podman Desktop

```
podman build --file ./Dockerfile --tag globaleshop.customers:latest ../

podman run --network=host --rm --name customers -p 8000:8000 globaleshop.customers -d
podman run --rm --name customers -p 8000:8000 globaleshop.customers -d
```

##### If you are using Docker and Docker Desktop

```
docker build --file ./Dockerfile --tag globaleshop.customers:latest ../

docker run --network=host --rm --name customers -p 8000:8000 globaleshop.customers -d
docker run --rm --name customers -p 8000:8000 globaleshop.customers -d
```

### EFCore Migrations

Go to Customer folder and run the following command to apply the migration:

```
dotnet ef migrations add Initial-Migration --project .\Infrastructure\Infrastructure.csproj --startup-project .\WebApi\WebApi.csproj --context CustomerDbContext
```