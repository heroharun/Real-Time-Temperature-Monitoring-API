Real Time Temperature Monitoring API
=====================================
This API is used to city wise temperature monitoring in real time. 
Mock test cases are also added to test project. The API is developed using .Net 8.0 and MongoDB. The API is dockerized and can be run using docker-compose.

## Features
- Real time temperature monitoring for multiple cities
- City wise temperature monitoring
- Average temperature monitoring
- Extreme temperature monitoring

## API Endpoints
- Get /api/temperature - Get temperature of all cities
- Post /api/temperature - Add temperatures of specific city
- Get /api/temperature/{city} {startDate} {endDate} - Get temperature of specific city and date range
- Get /api/temperature/{city} - Get average temperature of a city
- Get /api/temperature/{city}/extremes - Get extreme temperature of a city

## Installation
- Clone the repository
- install the dependencies using from nuget package manager
- sure mongodb is installed and running
- run the application

## Usage
- Get temperature of all cities
```
GET /api/temperature
```
- Add temperatures of specific city
``` 
POST /api/temperature
```
- Get temperature of specific city and date range
```
GET /api/temperature/{city} {startDate} {endDate}
```
- Get average temperature of a city
```
GET /api/temperature/{city}
```
- Get extreme temperature of a city
```
GET /api/temperature/{city}/extremes
```

Used Technologies
-----------------
- .Net 8.0
- MongoDB
- Swagger
- Docker
- Docker Compose

License
-------
MIT
