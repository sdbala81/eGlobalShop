#!/bin/bash

# eGlobalShop Podman Build Script
# This script builds all microservices using podman-compose
# Usage: ./build.sh [profile]
#   profile: optional parameter (infra, app, or omit for all services)

set -e

# Get the profile parameter
PROFILE=$1

if [ -n "$PROFILE" ]; then
    echo "🔨 Building eGlobalShop services (Profile: $PROFILE)..."
else
    echo "🔨 Building eGlobalShop services (All services)..."
fi

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Build services based on profile
if [ "$PROFILE" = "infra" ]; then
    echo "🔨 Building infrastructure services..."
    podman-compose -f compose.yaml -f podman-compose-infra.yaml build
elif [ "$PROFILE" = "app" ]; then
    echo "🔨 Building application services..."
    podman-compose -f compose.yaml -f podman-compose-app.yaml build
else
    echo "🔨 Building all services..."
    echo "Building infrastructure services..."
    podman-compose -f compose.yaml -f podman-compose-infra.yaml build
    echo "Building application services..."
    podman-compose -f compose.yaml -f podman-compose-app.yaml build
fi

echo "✅ Build completed successfully!"