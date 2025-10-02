# eGlobalShop AI Coding Agent Instructions

## Architecture Overview

This is a .NET 9 microservices solution with 5 independent services communicating via HTTP APIs. Each service follows **Minimal API + Endpoint Pattern** with containerization support.

**Service Boundaries & Port Allocation:**

- Order Service (5000/5001) - Order management with customer and item details
- Customer Service (7000/7001) - Customer profiles and addresses
- Inventory Service (6000/6001) - Product inventory management
- Payment Service (8000/8001) - Payment transaction handling
- Shipping Service (9000/9001) - Shipment tracking and logistics

**Key Pattern:** Services use in-memory data (no databases yet) with hardcoded sample data in endpoints for rapid prototyping.

## Project Structure & Conventions

```
services/src/{ServiceName}/
├── Program.cs              # Minimal API setup + endpoint registration
├── Endpoints/              # Extension methods for endpoint mapping
│   └── Get{Entity}Endpoint.cs
├── Models/                 # Domain models (no repositories yet)
└── Dockerfile             # Multi-stage .NET 9 build
```

**Endpoint Pattern:** All endpoints are static extension methods on `IEndpointRouteBuilder`:

```csharp
public static void MapGetAllOrders(this IEndpointRouteBuilder app)
{
    app.MapGet("/api/orders", () => { /* logic */ })
       .WithName("GetAllOrders")
       .WithOpenApi();
}
```

**Registration Pattern:** Services register endpoints in `Program.cs`:

```csharp
app.MapGetAllOrders();
app.MapGetOrderById();
```

## Critical Development Workflows

### Local Development

```bash
# Individual service (from service root)
cd services/src/Order && dotnet run

# All services via containers
cd podman && ./deploy.sh
# OR from project root: podman-compose -f podman/podman-compose.yaml up --build
```

### Kubernetes Deployment

```bash
cd k8s && ./deploy.sh
# Deploys to 'eglobalshop' namespace with proper sequencing
```

### API Testing

Use `eGlobalShop.http` with VS Code REST Client - contains all service endpoints with variables for easy port switching.

## Service Communication Patterns

**Currently:** Services are independent with no inter-service communication (each has sample data).

**Container Network:** All services run on `eglobalshop-network` bridge network when containerized.

**Kubernetes:** Services communicate via internal DNS (`http://service-name:port` pattern).

## Docker/Podman Specifics

**Image Naming:** `eglobalshop-{service}-service:latest` (e.g., `eglobalshop-order-service:latest`)

**Health Checks:** K8s deployments expect `/health` and `/ready` endpoints (not yet implemented - add these when extending services).

**Resource Limits:** K8s configured with 128Mi/100m requests, 256Mi/200m limits per service.

## When Adding New Services

1. Create service in `services/src/NewService/` following the Endpoint Pattern
2. Add to `podman/podman-compose.yaml` with next available port (10000+)
3. Create K8s manifests in `k8s/newservice/` (deployment.yaml, service.yaml, configmap.yaml)
4. Update both deployment scripts
5. Add API tests to `eGlobalShop.http`

## Integration Points

**OpenAPI:** All services expose `/openapi/v1.json` for schema discovery

**Environment:** Currently hardcoded to Development - modify compose/K8s configs for other environments

**Future Extensions:** The architecture is prepared for databases, service mesh, and inter-service communication but currently focuses on containerization and deployment patterns.
