# LTS.Candela

A full-stack application for managing users and their translation credits.

## Technologies Used

- **Backend:** .NET 9.0, Entity Framework Core, PostgreSQL
- **Frontend:** Vue 3, TypeScript, Vite, Vuetify
- **Containerization:** Docker, Docker Compose

## Project Structure

```
LTS.Candela.API/
├── Controllers/
│   └── UsersController.cs         # User management endpoints
├── Models/
│   └── User.cs                    # User entity definition
├── Data/
│   └── ApplicationDbContext.cs    # EF Core database context
├── Dtos/                          # Data transfer objects for User
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs # Global error handling
├── Migrations/                    # Database migrations
├── Program.cs                     # Application entry point
├── Dockerfile                     # Container build instructions
└── appsettings.json               # Application configuration

LTS.Candela.Frontend/
├── src/
│   ├── App.vue                    # Main Vue component
│   ├── main.ts                    # Application entry point
│   ├── assets/                    # Static assets (logo, etc.)
│   ├── components/                # Reusable Vue components
│   ├── layouts/                   # Layout components
│   ├── pages/                     # Page components (index, users, etc.)
│   ├── plugins/                   # Plugin initialization (e.g., Vuetify)
│   ├── router/                    # Vue Router setup
│   ├── services/                  # API service layer (userService.ts)
│   ├── stores/                    # Pinia stores (state management)
│   └── styles/                    # SCSS styles and settings
├── package.json                   # Project dependencies
├── tsconfig.json                  # TypeScript configuration
├── vite.config.mts                # Vite build configuration
├── Dockerfile                     # Container build instructions
└── index.html                     # HTML entry point
```

## Setup Instructions

1. **Prerequisites**
   - Docker Desktop
   - .NET 9.0 SDK (for backend local development)
   - Node.js 20+ (for frontend local development)

2. **Running the Application (Docker)**
   ```bash
   docker compose up -d --build
   ```
   This will start:
   - API service on port 8080
   - Frontend on port 5173
   - PostgreSQL database on port 5432

3. **Database**
   - Database Name: usermanager
   - User: postgres
   - Password: postgres
   - Connection string is automatically configured in docker-compose.yml

## API Endpoints

### User Management

- **Create User:** `POST /api/User`
- **Get User:** `GET /api/User/{id}`
- **Get All Users:** `GET /api/User`
- **Update User:** `PUT /api/User/{id}`
- **Delete User:** `DELETE /api/User/{id}`
- **Update Credits:** `PATCH /api/User/{id}/credits`

Request/response bodies are defined in the Dtos folder.

## Data Persistence

The application uses Docker volumes for data persistence:
- Volume Name: `postgres_data`
- Mount Point: `/var/lib/postgresql/data`
- Data survives container restarts and removals

## Docker Configuration

### Multi-stage Dockerfile (Backend)
- Uses .NET 9.0 SDK and ASP.NET runtime
- Restores dependencies, builds, and publishes the application
- Exposes ports 8080 and 8081

### Docker Compose

The `docker-compose.yml` configuration:
- Sets up API, frontend, and database services
- Configures environment variables
- Sets up port mappings
- Establishes dependencies and health checks
- Sets up volume for data persistence

## Health Monitoring

- API Health Check: `/health` endpoint
- Database Health Check: Uses `pg_isready`

## Environment Variables

The API service uses the following environment variables:
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ASPNETCORE_URLS`: http://+:8080
- `ASPNETCORE_HTTP_PORTS`: 8080
- `ConnectionStrings__DefaultConnection`: Database connection string

The frontend uses environment variables defined in `.env` files as needed.

## Recent Changes

- Added Vue 3 frontend with Vite, Pinia, and Vuetify.
- Refactored backend to use UsersController and Dtos.
- Added ExceptionHandlingMiddleware for global error handling.
- Added Dockerfile for frontend.
- Updated docker-compose.yml to orchestrate backend, frontend, and database.
- Added new migrations for user and credits.
- Improved project structure for scalability.
