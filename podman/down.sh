#!/bin/bash

# eGlobalShop Podman Cleanup Script
# This script stops and removes all eGlobalShop containers, networks, and volumes

set -e

echo "🧹 Cleaning up eGlobalShop resources..."

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Stop all services first
echo "🛑 Stopping all services..."
podman-compose -f compose.yaml -f podman-compose-infra.yaml -f podman-compose-app.yaml stop || echo "   Services already stopped or not running"

# Remove all services and their resources
echo "🗑️  Removing containers and associated resources..."
podman-compose -f compose.yaml -f podman-compose-infra.yaml -f podman-compose-app.yaml down || echo "   Compose stack already down"

# Remove any remaining eGlobalShop containers
echo "🗑️  Removing any remaining eGlobalShop containers..."
podman ps -a --filter "name=order-service" --filter "name=customers-service" --filter "name=inventory-service" --filter "name=billing-service" --filter "name=shipping-service" --filter "name=postgres-db" --filter "name=pgadmin" --filter "name=nats" --filter "name=seq" --format "{{.Names}}" | xargs -r podman rm -f

# Remove specific volumes defined in compose files
echo "💾 Removing eGlobalShop volumes..."
podman volume exists seq-data && podman volume rm seq-data || echo "   Volume seq-data not found (already removed)"

# Remove the eGlobalShop network
echo "🌐 Removing eGlobalShop network..."
podman network exists eglobalshop-network && podman network rm eglobalshop-network || echo "   Network eglobalshop-network not found (already removed)"

# Remove any additional dangling volumes
echo "💾 Removing any dangling volumes..."
podman volume ls -q --filter "dangling=true" | xargs -r podman volume rm

# Clean up any unused images (optional - uncomment if desired)
# echo "🖼️  Removing unused images..."
# podman image prune -f

echo ""
echo "✨ Cleanup completed successfully!"
echo ""

# Verify cleanup
echo "📊 Verifying cleanup..."
echo "Remaining eGlobalShop containers:"
podman ps -a --filter "name=eglobalshop" --filter "name=order-service" --filter "name=customers-service" --filter "name=inventory-service" --filter "name=billing-service" --filter "name=shipping-service" --filter "name=postgres-db" --filter "name=pgadmin" --filter "name=nats" --filter "name=seq" --format "table {{.Names}}\t{{.Status}}" || echo "✅ No eGlobalShop containers found"

echo ""
echo "Available networks:"
podman network ls --format "table {{.Name}}\t{{.Driver}}"

echo ""
echo "Available volumes:"
podman volume ls --format "table {{.Name}}\t{{.Driver}}"

echo ""
echo "🚀 To redeploy eGlobalShop, run:"
echo "   ./start.sh"