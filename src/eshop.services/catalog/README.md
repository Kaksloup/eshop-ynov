
# Catalog.API

## Description
Service responsable de la gestion du catalogue produit.

## Architecture
- Microservice
- Vertical Slice Architecture
- CQRS
- Repository Pattern

## Technologies
- ASP.NET Core Web API
- PostgreSQL
- Docker

## Endpoints
- POST /products
- GET /products/{id}
- GET /products?pageNumber=1&pageSize=10
- GET /products/category?category&pageNumber=1&pageSize=10
- PUT /products/{id}
- DELETE /products/{id}

## Lancer le projet
docker-compose up -d

