#!/bin/bash

# eGlobalShop Podman Deployment Script
# This script builds and starts all microservices using podman-compose
# Usage: ./start.sh [environment]
#   environment: required parameter (local, dev, or prod)

set -e

# Get parameters
ENVIRONMENT=$1

# Validate environment parameter
if [ -z "$ENVIRONMENT" ] || ([ "$ENVIRONMENT" != "local" ] && [ "$ENVIRONMENT" != "dev" ] && [ "$ENVIRONMENT" != "prod" ]); then
    echo "❌ Error: Environment parameter is required and must be one of: local, dev, or prod"
    echo "Usage: ./start.sh [local|dev|prod]"
    echo "  environment: required parameter (local, dev, or prod)"
    exit 1
fi

# Load environment variables using switch statement
case "$ENVIRONMENT" in
    "local")
        ENV_FILE="../.env.local"
        ;;
    "dev")
        ENV_FILE="../.env.dev"
        ;;
    "prod")
        ENV_FILE="../.env"
        ;;
    *)
        echo "❌ Error: Invalid environment '$ENVIRONMENT'. Must be one of: local, dev, prod"
        exit 1
        ;;
esac

if [ -f "$ENV_FILE" ]; then
    echo "📁 Loading environment from $ENV_FILE"
    export $(cat "$ENV_FILE" | grep -v '^#' | xargs)
else
    echo "❌ Error: Environment file $ENV_FILE not found"
    exit 1
fi

echo "🚀 Deploying eGlobalShop with Podman (Environment: $ENVIRONMENT)..."

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Build services first
echo "🔨 Building services..."
./build.sh "$ENVIRONMENT"

echo ""
echo "🚀 Starting services..."

# Start all services
echo "Starting infrastructure services first..."
podman-compose -f compose.yaml -f podman-compose-infra.yaml up -d
echo "Starting application services..."
podman-compose -f compose.yaml -f podman-compose-app.yaml up -d

echo ""
echo "🎉 All services deployed successfully!"
echo ""

# List all running containers
echo "📊 Container Status:"
podman ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

echo ""
echo "🌐 Service URLs:"
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

echo ""
echo "📝 To check logs for a specific service, use:"
echo "   podman logs <container-name>"
echo "   Example: podman logs order-service"
echo ""
echo "🔍 To check logs for all services:"
echo "   podman-compose -f compose.yaml -f podman-compose-infra.yaml logs -f"
echo "   podman-compose -f compose.yaml -f podman-compose-app.yaml logs -f"

echo ""
echo "🛑 To stop all services, run:"
echo "   ./down.sh $ENVIRONMENT"
echo ""
echo "📋 Usage examples:"
echo "   ./start.sh local               # Deploy all services locally"
echo "   ./start.sh dev                 # Deploy all services for development"
echo "   ./start.sh prod                # Deploy all services for production"
echo "   ./build.sh local               # Build all services only (local)"
echo "   ./build.sh dev                 # Build all services only (development)"
echo "   ./build.sh prod                # Build all services only (production)"
echo "   ./down.sh local                # Stop and cleanup (local)"
echo "   ./down.sh dev                  # Stop and cleanup (development)"
echo "   ./down.sh prod                 # Stop and cleanup (production)"