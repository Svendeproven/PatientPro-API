# PatientPro

![PatientPro a pro tool for healthcare.](exam-api-project/Documentation/logo-crop.png "PatientPro logo")

API for the PatientPro Cross platform app 



## Architecture and design

The code structure is made up of the following folders:

```
├───exam-api-project
│   ├───Controllers
│   ├───DoCumentation
│   ├───MiddleWares
│   ├───models
│   ├───Repositories
|   |───Services
│   └───Utilities
├───test-exam.test
│   ├───Controllers
│   ├───Services
```

The architecture is based on the MVC pattern. The Views are the PatientPro app the Models are the entityModels and the Controller is the Controllers
There is a Service layer to handle the business logic and a Repository layer to handle the data access.


## Getting started
This project is provided with a .env.example file which holds the environment variables needed for this project.

Copy the ```.env.example``` into a file named ```.env```

## Migrations
Migrations is located in the ```/Migrations``` folder.

To update the database with the migrations run the following command:
```sh
dotnet ef database update
```

To create a new migration run the following command:
```sh
dotnet ef migrations add InitialCreate
```

## Testing
This project uses xUnit for testing.

To run the tests run the following command from the test project folder ```/test-exam-api-test```:
```sh
dotnet test
```


## Deploy using docker-compose
This project can be deployed using docker-compose and an example file
Copy the ```docker-compose.yaml.example``` into a file named ```docker-compose.yaml ```
