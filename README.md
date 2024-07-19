# HangFireProjectScheduler
Project using HangFire scheduler

![GitHub repo size](https://img.shields.io/github/repo-size/Erickson-de-quadros-s/HangFireProjectScheduler)

## Description

This application is designed to schedule tasks using HangFire with Redis for storage and Swagger for API documentation.

## Motivation and Objective

### Motivation
The project was created to address the need for a robust task scheduling solution that integrates with Redis for storage and provides comprehensive API documentation. HangFire offers a powerful scheduling engine, but integrating it with Redis and ensuring proper API documentation were key goals to enhance functionality and usability.

### Objective
The primary objective of this project is to provide a reliable and scalable task scheduling system. By leveraging HangFire and Redis, the application aims to efficiently handle background jobs and tasks.


### Technologies implemented:

[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/download)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/get-started)
[![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)](https://redis.io/documentation)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=white)](https://swagger.io/docs/)
[![Hangfire](https://img.shields.io/badge/Hangfire-ff69b4?style=for-the-badge&logo=hangfire&logoColor=white)](https://www.hangfire.io/)

## Configuration

### TMDb API Key
To access the TMDb API, you need an API key. You can obtain a key by creating an account and generating a token on the [TMDb website](https://www.themoviedb.org/documentation/api). Once you have your API key, set it as an environment variable:

```bash
export TMDB_API_KEY=<your_tmdb_api_key>
```

## Installation

### Clone the repository:
```bash
$ git clone <https://github.com/Erickson-de-Quadros-s/HangFireProjectScheduler.git>
$ cd HangFireProjectScheduler
```

## Running the app
### development:

```bash
$ dotnet build
$ dotnet run
```

## Running the app with docker

### Prerequisites
Validate that Docker and Docker Compose are installed on your system. You can download and install Docker Desktop from [here](https://www.docker.com/products/docker-desktop).

```bash
$ docker-compose up --build

```

## Acess the application:
- Swagger UI: http://localhost:5025/swagger
- Hangfire Dashboard: http://localhost:5025/hangfireDashBoard



