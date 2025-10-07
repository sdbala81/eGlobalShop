#!/bin/bash

# eGlobalShop Podman Build Script
# This script builds all microservices using podman-compose
# Usage: ./build.sh [environment]
#   environment: required parameter (local, dev, or prod)

set -e

# Get parameters
ENVIRONMENT=$1

# Validate environment parameter
if [ -z "$ENVIRONMENT" ] || ([ "$ENVIRONMENT" != "local" ] && [ "$ENVIRONMENT" != "dev" ] && [ "$ENVIRONMENT" != "prod" ]); then
    echo "❌ Error: Environment parameter is required and must be one of: local, dev, or prod"
    echo "Usage: ./build.sh [local|dev|prod]"
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

echo "🔨 Building eGlobalShop services (Environment: $ENVIRONMENT)..."

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Build all services
echo "🔨 Building all services..."
podman-compose -f compose.yaml -f podman-compose-infra.yaml -f podman-compose-app.yaml build

echo "✅ Build completed successfully!"