# GoneSoon API

GoneSoon is a note-taking application where notes have a limited lifespan. Once deleted, they trigger a notification (email, SMS, or push) to remind the user about the task.

This project was created for educational purposes to explore clean architecture and for fun. 🚀

## Features
- Create self-expiring notes
- Store notes in Redis
- Notify users when notes expire
- User authentication and notification preferences
- REST API with Swagger documentation

## Technologies Used
- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** (SQL Server)
- **Redis** (for temporary storage)
- **RabbitMQ** (message queue for notifications)
- **Docker** (containerized deployment)
- **Swagger** (API documentation)

## Installation & Setup

### Prerequisites
- Docker & Docker Compose installed
- .NET 8 SDK (if running without Docker)

### Running with Docker
1. Clone the repository:
   ```sh
   git clone <repo-url>
   cd GoneSoon
   ```
2. Start the services:
   ```sh
   docker-compose up --build
   ```
3. Access the API at: `http://localhost:5000/swagger`

### Running Locally
1. Set up SQL Server and Redis manually.
2. Configure the `appsettings.json` file:
   ```json
   "ConnectionStrings": {
     "SqlServer": "Server=localhost,1433;Database=GoneSoonDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;",
     "Redis": "localhost:6379"
   }
   ```
3. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
4. Start the API:
   ```sh
   dotnet run --project ApplicationManager.Api
   ```

## API Endpoints
| Method  | Endpoint                     | Description                     |
|---------|------------------------------|---------------------------------|
| `POST`  | `/api/application`           | Create a new application        |
| `POST`  | `/api/application/{id}/handle` | Mark an application as handled |
| `POST`  | `/api/application/{id}/send-to-manager` | Send application to a manager |
| `POST`  | `/api/application/{id}/send-to-client` | Send application to a client  |

## Environment Variables
| Variable                  | Description                        |
|---------------------------|------------------------------------|
| `ConnectionStrings__SqlServer` | SQL Server connection string   |
| `ConnectionStrings__Redis`     | Redis connection string        |

## Contributing
Feel free to fork the repository and submit pull requests.

## License
MIT License

