# MeetupAPI <br><br>

## CRUD Web API for managing events
Built using .NET Core, EF Core, MS SQL Server, AutoMapper, FluentValidation and Authentication via bearer token with IdentityServer.

## How to use? <br><br>

* Clone this repository to your local machine
* Run the API
* The application checks if the database provider is SQL Server and applies any pending migrations
* Initial test data will be seeded
* Before making requests to authenticated endpoints, you need to obtain a jwt token 
* You can generate a jwt token using the IdentityServerAPI
   * Use Postman to send a POST request to the access token endpoint as follows:
       > https://localhost:IdentityServerUrlHere/connect/token
   * Pass here parameters like grant_type, scope, client_id and secret
* Then you can use API testing tools to interact with the endpoints and perform CRUD operations on entities, like this:
    
       GET /api/events     
           /api/sponsors     
           /api/speakers     
 
       or  /api/events/{id}
  
