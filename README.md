# LTS.Candela API

A .NET API service for managing users and their translation credits.

## Technologies Used

- **.NET 9.0**: Modern, cross-platform framework for building APIs
- **PostgreSQL**: Robust, open-source database for data persistence
- **Entity Framework Core**: ORM for database operations
- **Docker**: Containerization for consistent development and deployment
- **Docker Compose**: Multi-container application orchestration

## Project Structure

```
LTS.Candela.API/
├── Controllers/
│   └── UserController.cs     # User management endpoints
├── Models/
│   └── User.cs              # User entity definition
├── Data/
│   └── ApplicationDbContext.cs  # EF Core database context
├── Migrations/              # Database migrations
├── Program.cs              # Application entry point
├── Dockerfile             # Container build instructions
└── appsettings.json      # Application configuration
```

## Setup Instructions

1. **Prerequisites**
   - Docker Desktop
   - .NET 9.0 SDK (for local development)

2. **Running the Application**
   ```bash
   docker compose up -d
   ```
   This will start:
   - API service on port 8080
   - PostgreSQL database on port 5432

3. **Database**
   - Database Name: usermanager
   - User: postgres
   - Password: postgres
   - Connection string is automatically configured in docker-compose.yml

## API Endpoints

### User Management

1. **Create User**
   - POST `/api/User`
   - Body:
     ```json
     {
       "name": "string",
       "email": "string",
       "translationCredits": integer
     }
     ```

2. **Get User**
   - GET `/api/User/{id}`

3. **Get All Users**
   - GET `/api/User`

4. **Update User**
   - PUT `/api/User/{id}`
   - Body: Same as create

5. **Delete User**
   - DELETE `/api/User/{id}`

6. **Update Credits**
   - PATCH `/api/User/{id}/credits`
   - Body: integer (new credit amount)

## Data Persistence

The application uses Docker volumes for data persistence:
- Volume Name: `postgres_data`
- Mount Point: `/var/lib/postgresql/data`
- Data survives container restarts and removals

## Docker Configuration

### Multi-stage Dockerfile
1. **Base Stage**:
   - Uses .NET 9.0 ASP.NET runtime
   - Exposes ports 8080 and 8081

2. **Build Stage**:
   - Uses .NET 9.0 SDK
   - Restores dependencies
   - Builds the application

3. **Publish Stage**:
   - Publishes the application

4. **Final Stage**:
   - Creates the runtime image
   - Copies published application

### Docker Compose

The `docker-compose.yml` configuration:
- Sets up API and database services
- Configures environment variables
- Sets up port mappings
- Establishes dependencies
- Configures health checks
- Sets up volume for data persistence

## Health Monitoring

- API Health Check:
  - Endpoint: `/health`
  - Interval: 30s
  - Timeout: 10s
  - Retries: 3

- Database Health Check:
  - Uses `pg_isready`
  - Interval: 10s
  - Timeout: 5s
  - Retries: 5

## Environment Variables

The API service uses the following environment variables:
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ASPNETCORE_URLS`: http://+:8080
- `ASPNETCORE_HTTP_PORTS`: 8080
- `ConnectionStrings__DefaultConnection`: Database connection string
