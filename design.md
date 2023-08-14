# Design

A demo microservice project running in containers on k8s.
Project will store permanent data in a postgres database, but
will store session data in redis.

The application consists of:

- An EF Core project for postgres db access
- A _backend_ microservice
- A _frontend_ microservice (or services)

## Store

This is a project to sell products.

## Todos

- [ ] Build EF Core project with connection to AWS