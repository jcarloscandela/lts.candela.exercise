# LTS.Candela Exercise

This project consists of a backend API built with ASP.NET Core and a frontend built with Vue 3, TypeScript, and Vite. Both are containerized with Docker and orchestrated using Docker Compose.

---

## Project Structure

```
.
├── LTS.Candela.API/           # Backend (.NET Core API)
│   ├── LTS.Candela.API/       # API source code
│   └── LTS.Candela.API.Tests/ # API unit tests
├── LTS.Candela.Frontend/      # Frontend (Vue 3 + Vite + TypeScript)
│   ├── src/                   # Source code (components, pages, models, services)
│   ├── public/                # Static assets
│   ├── package.json           # Frontend dependencies
│   └── Dockerfile             # Dockerfile for frontend
├── docker-compose.yml         # Multi-service orchestration
└── README.md                  # Project documentation
```

---

## Prerequisites

- [Docker](https://www.docker.com/)
- [Node.js](https://nodejs.org/) (for local frontend development)
- [.NET 9 SDK](https://dotnet.microsoft.com/) (for local backend development)

---

## Running with Docker Compose

To build and run both frontend and backend:

```sh
docker-compose up --build
```

- The frontend will be available at [http://localhost:5173](http://localhost:5173)
- The backend API will be available at [http://localhost:5000](http://localhost:5000) (or as configured)

---

## Frontend

- **Framework:** Vue 3 + Vite + TypeScript
- **Directory:** `LTS.Candela.Frontend/`

### Structure

- `src/components/` – Vue components
- `src/pages/` – Page components (e.g., users)
- `src/models/` – TypeScript models
- `src/services/` – API service modules
- `src/router/` – Vue Router setup
- `src/App.vue` – Root component
- `src/main.ts` – App entry point

### Local Development

```sh
cd LTS.Candela.Frontend
npm install
npm run dev
```

The app will be available at [http://localhost:5173](http://localhost:5173).

---

## Backend

- **Framework:** ASP.NET Core (.NET 8)
- **Directory:** `LTS.Candela.API/`

### Structure

- `LTS.Candela.API/` – API source code
- `LTS.Candela.API.Tests/` – Unit tests

### Local Development

```sh
cd LTS.Candela.API/LTS.Candela.API
dotnet restore
dotnet run
```

The API will be available at [http://localhost:5000](http://localhost:5000).

---

## Running Tests

### Backend

```sh
cd LTS.Candela.API/LTS.Candela.API.Tests
dotnet test
```

---

## Environment Variables

- Frontend: Configure `.env` in `LTS.Candela.Frontend/` as needed.
- Backend: Configure `appsettings.json` or `appsettings.Development.json` in `LTS.Candela.API/LTS.Candela.API/`.

---

## Future Improvements

- Kubernetes (k8s) local development and deployment support
- Authentication (user login, registration)
- Authorization (role-based access control)
- Multilanguage (i18n) support
- End-to-end (E2E) testing
- Integration testing
- Create Dockerfile for running Unit Tests
