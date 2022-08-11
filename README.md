# desafioV2
C#, .Net 6.0, EntityFrameWorkCore 6.0, CodeFirst, Postgres, CQRS, RabbitMQ/MassTransit, MongoDb

This repository is an updated version of an Internship challenge (https://github.com/aleteles3/desafio_alexandre), done solely for study purposes.

Structure-wise, it consists of an Onion Architecture, with Core projects to act as reference "packages". It has WebApi REST services, using .NET 6.0 and EntityFrameWorkCore 6.0. 
CQRS (Command Query Responsiblity Segregation) was achieved using MediatR. 
The test database was Postgres, using CodeFirst migrations.
Unit Tests were done with XUnit. Authorization was made using JWT Bearer Token. The RefreshTokens are stored in a MongoDb database.
A small business flow was implemented using RabbitMQ along with MassTransit as messaging queueing broker. The user places an order, and the order is enqueued, to be accepted/declined later, in an asynchronous manner.
