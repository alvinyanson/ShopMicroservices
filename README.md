# .NET Microservices

## Overview

**AuthService** - Manages account-related operations, including login, password changes, and authentication.

**ProductCatalogService** - Oversees all operations related to products, categories, and transactions.

## Clone the Repository

Open your terminal and run the following command to clone the repository:

    git clone https://github.com/alvinyanson/ShopMicroservices

After cloning, open the solution the `ShopMicroservices.sln`

## Task 1 - Environment Setup

    NET SDK: 8.0.303
    Docker Desktop: 4.33.0 (160616)
    Kubernetes: v1.30.2
    SQL Server Management Studio: 20.1.10.0


## Task 2 - Develop 2 Domain Specific Microservices

### Microservice 1 - AuthService

#### Running the AuthService locally

Make sure you are in the AuthService directory. If not, run the following command in the terminal:

    cd AuthService

In terminal, run the `AuthService`:
    
    dotnet run

Access the services on Postman:

    http://localhost:5174/api/Auth/Login
    http://localhost:5174/api/Auth/Logout
    http://localhost:5174/api/Auth/Register
    http://localhost:5174/api/Auth/ChangePassword
    http://localhost:5174/api/Auth/ChangeEmailAndUsername

### Microservice 2 - ProductCatalogService

#### Running the ProductCatalogService locally

Make sure you are in the ProductCatalogService directory. If not, run the following command in the terminal:

    cd ProductCatalogService

In terminal, run the `ProductCatalogService`:

    dotnet run

Access the service on Postman:

    http://localhost:5174/api/Products
    http://localhost:5174/api/Categories
    http://localhost:5174/api/Carts   

## Task 3 - Persistence Layer Implementation

At this point, it is expected that SQL Server is already installed on your local machine.

When you run both services, the migration is applied behind the scenes. You can find these files in:

    AuthService/Data/PrepDb.cs
    ProductCatalogService/Data/PrepDb.cs
    
You can find the relevant data access operations under the `Controllers` directory.

    AuthService/Controllers/AuthController
    ProductCatalogService/Controllers/Products
    ProductCatalogService/Controllers/Categories
    ProductCatalogService/Controllers/Carts

## Task 4 - Kubernetes Deployment

All YAML configuration files can be found under the `K8S/` directory.

### Auth Service Deployment

In terminal, run the command:

    kubectl apply -f auth-depl.yaml


After successful deployment, the AuthService can be accessed using the following details:

    Service Name: auth-clusterip-srv
    Port: 8080
    Protocol: TCP

Open docker desktop and verify successful deployment.

   ![AuthService Screenshot](https://raw.githubusercontent.com/alvinyanson/ShopMicroservices/master/Docs/AuthService.png)


### ProductCatalogService Deployment

In terminal, run the command:
 
    kubectl apply -f product-catalog-depl.yaml


After successful deployment, the `ProductCatalogService` can be accessed using the following details:

    Service Name: product-catalog-clusterip-srv
    Port: 8080
    Protocol: TCP

Open docker desktop and verify successful deployment.

   ![ProductCatalogService Screenshot](https://raw.githubusercontent.com/alvinyanson/ShopMicroservices/master/Docs/ProductCatalogService.png)

### MYSQL Deployment and Persistent Volume Storage

Since we require access to persistent volume storage for our database, we need to request storage. Kubernetes will then provision the requested storage.

In terminal, run the following command:
    
    kubectl apply -f local-pvc.yaml
   

Next, when setting up SQL Server in the Kubernetes cluster, an administrator password is required. Create a Secret for the SQL Server password. In our case, the password used is `passw0rd!`.

In terminal, run the following command:

    kubectl create secret generic mssql --from-literal=SA_PASSWORD="passw0rd!"

After creating the secret, we will define a single replica of a container running the `MSSQL Server` and include a persistent volume claim which will be mounted using PVC named `mssql-claim`. 

In terminal, run the following command:

    kubectl apply -f mssql-plat-depl.yaml

After successful deployment, you should be able to see the running container for MSSQL with the following details:

    - Service Name: mssql-clusterip-srv
    - Port: 1433
    - Protocol: TCP

    - Service Name: mssql-loadbalancer
    - Port: 1433
    - Protocol: TCP

Open docker desktop and verify successful deployment.

   ![MySql Screenshot](https://raw.githubusercontent.com/alvinyanson/ShopMicroservices/master/Docs/MySQL.png)



### Rabbit MQ Deployment

In our app, we will use asynchronous communication between services by enabling them to send and receive messages through a message bus. RabbitMQ will be employed for this purpose. Below is an overview of how our app will utilize RabbitMQ.

    - Publisher: AuthService
    - Subscriber: ProductCatalogService
    - Event: User registers, notify Product Catalog Service


In terminal, run the following command:
    
    kubectl apply -f rabbitmq-depl.yaml

Open docker desktop and verify successful deployment.

   ![RabbitMQ Screenshot](https://raw.githubusercontent.com/alvinyanson/ShopMicroservices/master/Docs/RabbitMQ.png)

Access the RabbitMQ Admin UI with the the following credentials. 

    - Endpoint: http://localhost:15672/
    - Username: guest
    - Password: guest

Open on any browser http://localhost:15672, then login using the credentials mentioned above.

![RabbitMQUI Screenshot](https://raw.githubusercontent.com/alvinyanson/ShopMicroservices/master/Docs/RabbitMQUI.png)


### API Gateway Deployment (Ingress Nginx)

Ingress will act as a reverse proxy and load balancer, handling external traffic and routing it to our specified services.

In terminal, run the following command:

    kubectl apply -f ingress-srv.yaml


Verify the Deployment using the command below. Execute one at a time.

    kubectl get deploy --namespace=ingress-nginx
    kubectl get svc --namespace=ingress-nginx


Open docker desktop and verify successful deployment.

  ![IngressController Screenshot](https://raw.githubusercontent.com/alvinyanson/ShopMicroservices/master/Docs/IngressController.png)



## Task 5 - API Gateway Integration

In our case, I've chosen to use Ingress Nginx as the API gateway for our microservices.

Our configuration sets up routing rules for incoming HTTP traffic based on hostname and path prefixes, directing requests to specific services within the Kubernetes cluster.

Open the `ingress-srv.yaml file`. In the host property, I've configured the hostname as `shopme.com`. All HTTP traffic from this hostname will be routed to the specified services.


    AuthService
      https://shopme.com/api/Auth

    ProductCatalogService
      https://shopme.com/api/Products
      https://shopme.com/api/Categories
      https://shopme.com/api/Carts


## Task 6 - Synchronous Communication

In our app, the implementation of synchronous communication is described below.

When a user adds an item to their cart, the request first communicates with `AuthService` to retrieve the `ownerId` of the currently logged-in user by passing the token provided at login. `AuthService` then returns the claim for this token, specifically the `ownerId`, back to `ProductCatalogService`.

For synchronous communication to function correctly, `ProductCatalogService` needs to know the IP address of the receiving `AuthService`.

In `ProductCatalogService`, verify that you have the following configurations.

  `appsettings.Development.json`

    "ConnectionStrings": {
      "AuthService": "http://localhost:5174/api/Auth"
    }

  `appsettings.Production.json`

    "ConnectionStrings": {
      "AuthService": "http://auth-clusterip-srv:8080/api/Auth"
    }


## Task 7 - Asynchronous Messaging with Event Bus

In our app, the implementation of asynchronous communication is described below.

When a user registers, we'll send a notification to the `ProductCatalogService` to indicate that a new user has registered. Currently, we'll log the received message. In the future, we could enhance this by using the registration email to send a welcome message or promotional offers.


    Publisher: AuthService,
    Subscriber: ProductCatalogService
    Event: User registers, notify Product Catalog Service


We have set up RabbitMQ with a single replica, exposing management and messaging ports internally through a ClusterIP service. This ClusterIP service facilitates communication between RabbitMQ and other services within the Kubernetes cluster.


In `AuthService` and `ProductCatalogService`, verify that you have the following configurations.

`appsettings.Development.json`

    "RabbitMQHost": "localhost",
    "RabbitMQPort": "5672"


`appsettings.Production.json`

    "RabbitMQHost": "rabbitmq-clusterip-srv",
    "RabbitMQPort": "5672"
