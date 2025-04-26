# supreme-succotash

Health system

# Database

postgresql database will be used for data storage of this system.

# Backend Webapi (c#)

The project follows the microservices architecture for the api's. Dapper and JOzykql.pg orm's are used. Both rest api and graphql will be used where graphql will be used for fetching data from the database and rest api will be used for the sign in and sign up and the api for the clients profile.

All the CRUD operations will consume graphql endpoints.

The CRUD operations handled by graphql are
--Create client
--create programs to a client
--geting clients
--getting clients by id
-- creating and fetching setups
(gender, country, county, subcounty, roles)

The Rest api end points are in user signup and user login.
