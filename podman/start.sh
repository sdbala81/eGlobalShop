#!/bin/bash

# eGlobalShop Podman Deployment Script
# This script builds and starts all microservices using podman-compose
# Usage: ./deploy.sh [profile]
#   profile: optional parameter (infra, app, or omit for all services)

set -e

# Get the profile parameter
PROFILE=$1

if [ -n "$PROFILE" ]; then
    echo "🚀 Deploying eGlobalShop with Podman (Profile: $PROFILE)..."
else
    echo "🚀 Deploying eGlobalShop with Podman (All services)..."
fi

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Build and start services based on profile
if [ "$PROFILE" = "infra" ]; then
    echo "🔨 Building and starting infrastructure services..."
    podman-compose -f compose.yaml -f podman-compose-infra.yaml up --build -d
elif [ "$PROFILE" = "app" ]; then
    echo "🔨 Building and starting application services..."
    podman-compose -f compose.yaml -f podman-compose-app.yaml up --build -d
else
    echo "🔨 Building and starting all services..."
    echo "Starting infrastructure services first..."
    podman-compose -f compose.yaml -f podman-compose-infra.yaml up --build -d
    echo "Starting application services..."
    podman-compose -f compose.yaml -f podman-compose-app.yaml up --build -d
fi

echo ""
echo "🎉 All services deployed successfully!"
echo ""

# List all running containers
echo "📊 Container Status:"
podman ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

echo ""
echo "🌐 Service URLs:"
if [ "$PROFILE" = "infra" ]; then
    echo "   PostgreSQL:        http://localhost:5432"
    echo "   PgAdmin:           http://localhost:5050"
    echo "   NATS:              http://localhost:4222"
    echo "   NATS Monitor:      http://localhost:8222"
    echo "   Seq:               http://localhost:5341"
elif [ "$PROFILE" = "app" ]; then
    echo "   Order Service:     http://localhost:5000"
    echo "   Inventory Service: http://localhost:6000"
    echo "   Customer Service:  http://localhost:7000"
    echo "   Shipping Service:  http://localhost:8000"
    echo "   Billing Service:   http://localhost:9000"
else
    echo "   Order Service:     http://localhost:5000"
    echo "   Inventory Service: http://localhost:6000"
    echo "   Customer Service:  http://localhost:7000"
    echo "   Shipping Service:  http://localhost:8000"
    echo "   Billing Service:   http://localhost:9000"
    echo ""
    echo "   Infrastructure:"
    echo "   PostgreSQL:        http://localhost:5432"
    echo "   PgAdmin:           http://localhost:5050"
    echo "   NATS:              http://localhost:4222"
    echo "   NATS Monitor:      http://localhost:8222"
    echo "   Seq:               http://localhost:5341"
fi

echo ""
echo "📝 To check logs for a specific service, use:"
echo "   podman logs <container-name>"
echo "   Example: podman logs order-service"
echo ""
echo "🔍 To check logs for all services:"
if [ "$PROFILE" = "infra" ]; then
    echo "   podman-compose -f compose.yaml -f podman-compose-infra.yaml logs -f"
elif [ "$PROFILE" = "app" ]; then
    echo "   podman-compose -f compose.yaml -f podman-compose-app.yaml logs -f"
else
    echo "   podman-compose -f compose.yaml -f podman-compose-infra.yaml logs -f"
    echo "   podman-compose -f compose.yaml -f podman-compose-app.yaml logs -f"
fi

echo ""
echo "🛑 To stop all services, run:"
echo "   ./cleanup.sh"
echo ""
echo "📋 Usage examples:"
echo "   ./deploy.sh                    # Deploy all services"
echo "   ./deploy.sh infra              # Deploy only infrastructure services"
echo "   ./deploy.sh app                # Deploy only application services"