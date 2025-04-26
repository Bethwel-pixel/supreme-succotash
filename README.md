Supreme Succotash
Supreme Succotash is a health system designed using a microservices architecture.
It uses PostgreSQL as the main database and provides services through both REST APIs and GraphQL.

Database
Database: PostgreSQL

Description: All system data, including users, clients, programs, and system setups, is stored in PostgreSQL.

Web API (C#)
Framework: ASP.NET Core Web API

Architecture: Microservices

ORMs:

Dapper: A lightweight and fast ORM for database operations.

Jozykql.pg: An advanced ORM for PostgreSQL with GraphQL support.

Communication:

GraphQL: Used mainly for data operations (CRUD).

REST API: Used for user authentication (sign-up, login) and profile management.

APIs
GraphQL
Handled through GraphQL:

Clients

Create a client

Get all clients

Get a client by ID

Programs

Create programs assigned to clients

Setups

Create and retrieve system setups such as:

Gender

Country

County

Subcounty

Roles

REST
Handled through REST:

Authentication

Sign Up

Sign In

Profile Management

Technologies Used
ASP.NET Core Web API

PostgreSQL

Dapper

Jozykql.pg

GraphQL

RESTful API

Ocelot API Gateway

Notes
Authentication is handled using JWT Bearer tokens.

Authorization is role-based across the system.

Microservices are routed through an API Gateway (Ocelot).

Each service exposes its own /graphql endpoint for GraphQL operations.

The authentication service provides login and signup functionalities via REST endpoints.
