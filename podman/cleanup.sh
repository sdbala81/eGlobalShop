#!/bin/bash

# eGlobalShop Podman Cleanup Script
# This script stops and removes all eGlobalShop containers, networks, and volumes

set -e

echo "🧹 Cleaning up eGlobalShop resources..."

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Stop and remove all services
echo "🛑 Stopping and removing containers..."
podman-compose down

# Remove any dangling containers related to eGlobalShop
echo "🗑️  Removing any remaining eGlobalShop containers..."
podman ps -a --filter "name=order-service" --filter "name=customer-service" --filter "name=inventory-service" --filter "name=payment-service" --filter "name=shipping-service" --format "{{.Names}}" | xargs -r podman rm -f

# Remove the network if it exists
echo "🌐 Removing eGlobalShop network..."
podman network exists eglobalshop-network && podman network rm eglobalshop-network || echo "   Network eglobalshop-network not found (already removed)"

# Remove any volumes created by the deployment
echo "💾 Removing any anonymous volumes..."
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
podman ps -a --filter "name=eglobalshop" --filter "name=order-service" --filter "name=customer-service" --filter "name=inventory-service" --filter "name=payment-service" --filter "name=shipping-service" --format "table {{.Names}}\t{{.Status}}" || echo "✅ No eGlobalShop containers found"

echo ""
echo "Available networks:"
podman network ls --format "table {{.Name}}\t{{.Driver}}"

echo ""
echo "🚀 To redeploy eGlobalShop, run:"
echo "   ./deploy.sh"