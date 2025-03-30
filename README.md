# GoneSoon

GoneSoon is a note management application with automatic note deletion after expiration. Notes are stored in Redis, and upon expiration, notifications are sent to users. This is an educational project demonstrating the use of Clean Architecture, Redis, SQL Server, background services, and event-driven architecture.

## Technologies

- **.NET 6/7**
- **Redis** — for storing notes and tracking their expiration.
- **SQL Server** — for storing user data.
- **Docker** — for containerizing the application (Redis, SQL Server, API).
- **MediatR** — for organizing communication between components.
- **ILogger** — for logging events.
- **Unit Tests** — for testing key components.

## Running the Project

For local development, use Docker to run the services.

1. **Install Docker**: [Docker Installation Guide](https://docs.docker.com/get-docker/).
2. **Clone the repository**:
   ```bash
   git clone https://github.com/007on/GoneSoon.git
   cd GoneSoon
