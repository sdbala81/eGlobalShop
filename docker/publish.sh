#!/bin/bash

# eGlobalShop Podman Publish Script
# This script pushes all microservice images to the configured container registry

set -e

# Load environment variables from .env file if it exists
if [ -f "$(dirname "$0")/../.env" ]; then
    echo "📄 Loading environment variables from .env file..."
    set -o allexport
    source "$(dirname "$0")/../.env"
    set +o allexport
fi

# Configuration
REGISTRY="${CONTAINER_REGISTRY:-docker.io}"
REGISTRY_USERNAME="${CONTAINER_REGISTRY_USERNAME}"
REGISTRY_PASSWORD="${CONTAINER_REGISTRY_PASSWORD}"
NAMESPACE="${CONTAINER_NAMESPACE:-eglobalshop}"

echo "📦 Publishing eGlobalShop images..."

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Check if credentials are provided for docker.io registry
if [[ "$REGISTRY" == "docker.io" || "$REGISTRY" == *"docker.io"* ]]; then
    if [[ -z "$REGISTRY_USERNAME" || -z "$REGISTRY_PASSWORD" ]]; then
        echo "❌ Error: Docker registry credentials are required!"
        echo "Please set the following environment variables in .env file in the root directory:"
        echo "   CONTAINER_REGISTRY_USERNAME=your-docker-username"
        echo "   CONTAINER_REGISTRY_PASSWORD=your-docker-password-or-token"
        echo ""
        echo "Example .env file:"
        echo "CONTAINER_REGISTRY_USERNAME=myusername"
        echo "CONTAINER_REGISTRY_PASSWORD=mypassword"
        exit 1
    fi
    
    echo "🔐 Logging into Docker registry..."
    echo "$REGISTRY_PASSWORD" | podman login --username "$REGISTRY_USERNAME" --password-stdin "$REGISTRY"
    
    if [ $? -eq 0 ]; then
        echo "✅ Successfully logged into Docker registry"
    else
        echo "❌ Failed to login to Docker registry"
        exit 1
    fi
fi

# Define the services and their corresponding images
declare -A SERVICES=(
    ["order-service"]="localhost/eglobalshop-order-service:latest"
    ["customers-service"]="localhost/eglobalshop-customers-service:latest"
    ["inventory-service"]="localhost/eglobalshop-inventory-service:latest"
    ["billing-service"]="localhost/eglobalshop-billing-service:latest"
    ["shipping-service"]="localhost/eglobalshop-shipping-service:latest"
)

echo ""
echo "🎯 Target Registry: $REGISTRY"
echo "📁 Namespace: $NAMESPACE"
echo ""

# Function to tag and push an image
publish_image() {
    local service_name=$1
    local local_image=$2
    local remote_tag
    
    # For docker.io registry, use username/namespace/service format
    if [[ "$REGISTRY" == "docker.io" && -n "$REGISTRY_USERNAME" ]]; then
        remote_tag="${REGISTRY}/${REGISTRY_USERNAME}/${NAMESPACE}-${service_name}:latest"
    else
        remote_tag="${REGISTRY}/${NAMESPACE}/${service_name}:latest"
    fi
    
    echo "🏷️  Tagging $local_image as $remote_tag"
    podman tag "$local_image" "$remote_tag"
    
    echo "⬆️  Pushing $remote_tag"
    podman push "$remote_tag"
    
    echo "✅ Successfully published $service_name"
    echo ""
}

# Check if all images exist locally
echo "🔍 Checking if all images exist locally..."
missing_images=()
for service in "${!SERVICES[@]}"; do
    image="${SERVICES[$service]}"
    if ! podman image exists "$image"; then
        missing_images+=("$image")
    fi
done

if [ ${#missing_images[@]} -ne 0 ]; then
    echo "❌ The following images are missing locally:"
    for image in "${missing_images[@]}"; do
        echo "   - $image"
    done
    echo ""
    echo "💡 To build all images, run:"
    echo "   ./deploy.sh"
    echo ""
    echo "⚠️  Or to build images without starting containers:"
    echo "   podman-compose build"
    exit 1
fi

echo "✅ All images found locally"
echo ""

# Publish each service
for service in "${!SERVICES[@]}"; do
    image="${SERVICES[$service]}"
    echo "📤 Publishing $service..."
    publish_image "$service" "$image"
done

echo "🎉 All images published successfully!"
echo ""

# Display published images
echo "📊 Published Images:"
for service in "${!SERVICES[@]}"; do
    if [[ "$REGISTRY" == "docker.io" && -n "$REGISTRY_USERNAME" ]]; then
        remote_tag="${REGISTRY}/${REGISTRY_USERNAME}/${NAMESPACE}-${service}:latest"
    else
        remote_tag="${REGISTRY}/${NAMESPACE}/${service}:latest"
    fi
    echo "   - $remote_tag"
done

echo ""
echo "💡 Environment Variables:"
echo "   CONTAINER_REGISTRY: Set to override registry (default: docker.io)"
echo "   CONTAINER_REGISTRY_USERNAME: Docker Hub username (required for docker.io)"
echo "   CONTAINER_REGISTRY_PASSWORD: Docker Hub password or token (required for docker.io)"
echo "   CONTAINER_NAMESPACE: Set to override namespace (default: eglobalshop)"
echo ""
echo "🔧 Example usage with custom registry:"
echo "   CONTAINER_REGISTRY=my-registry.com ./publish.sh"
echo ""
echo "📄 For Docker Hub, create a .env file in the root directory with:"
echo "   CONTAINER_REGISTRY_USERNAME=your-username"
echo "   CONTAINER_REGISTRY_PASSWORD=your-password-or-token"
echo "   CONTAINER_NAMESPACE=your-namespace  # Optional, defaults to eglobalshop"