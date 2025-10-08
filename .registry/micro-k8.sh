#!/bin/bash

# eGlobalShop MicroK8s Image Import Script
# This script imports all microservice images from local podman registry to MicroK8s
# It always imports the latest versions, overwriting any existing images in MicroK8s

set -e

echo "📦 Importing eGlobalShop images to MicroK8s (force latest)..."

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Check if MicroK8s is installed and running
if ! command -v microk8s &> /dev/null; then
    echo "❌ Error: MicroK8s is not installed or not in PATH!"
    echo "Please install MicroK8s first: https://microk8s.io/"
    exit 1
fi

# Check if MicroK8s is running
if ! microk8s status --wait-ready >/dev/null 2>&1; then
    echo "❌ Error: MicroK8s is not running!"
    echo "Start MicroK8s with: microk8s start"
    exit 1
fi

echo "✅ MicroK8s is running and ready"

# Define the services and their corresponding images
declare -A SERVICES=(
    ["orders"]="localhost/eglobalshop-order-service:latest"
    ["customers"]="localhost/eglobalshop-customers-service:latest"
    ["inventory"]="localhost/eglobalshop-inventory-service:latest"
    ["billing"]="localhost/eglobalshop-billing-service:latest"
    ["shipping"]="localhost/eglobalshop-shipping-service:latest"
)

echo ""
echo "🎯 Target: MicroK8s local registry"
echo ""

# Function to save and import an image (always overwrites existing images)
import_image() {
    local service_name=$1
    local local_image=$2
    local temp_file="/tmp/${service_name}.tar"
    
    echo "🔍 Checking if $local_image exists locally..."
    if ! podman image exists "$local_image"; then
        echo "❌ Error: Image $local_image not found locally!"
        echo "💡 Build the images first with: cd ../.scripts/bash && ./build.sh"
        return 1
    fi
    
    # Check if image already exists in MicroK8s and remove it to force fresh import
    local image_name="${local_image##*/}"
    local existing_images=$(microk8s ctr images ls | grep "$image_name" | awk '{print $1}' || true)
    if [ -n "$existing_images" ]; then
        echo "🔄 Removing existing images matching $image_name from MicroK8s to force fresh import..."
        echo "$existing_images" | while read -r existing_image; do
            if [ -n "$existing_image" ]; then
                echo "   - Removing: $existing_image"
                microk8s ctr image remove "$existing_image" || true
            fi
        done
    fi
    
    echo "💾 Saving $local_image to temporary file..."
    podman save "$local_image" -o "$temp_file"
    
    echo "⬆️  Importing $local_image to MicroK8s (force overwrite)..."
    microk8s ctr image import "$temp_file"
    
    echo "🧹 Cleaning up temporary file..."
    rm -f "$temp_file"
    
    echo "✅ Successfully imported $service_name (latest version)"
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
    echo "   cd ../.scripts/bash && ./build.sh"
    echo ""
    echo "⚠️  Or to build images without starting containers:"
    echo "   cd ../podman && podman-compose build"
    exit 1
fi

echo "✅ All images found locally"
echo ""

# Import each service
for service in "${!SERVICES[@]}"; do
    image="${SERVICES[$service]}"
    echo "📤 Importing $service..."
    import_image "$service" "$image"
done

echo "🎉 All images imported successfully to MicroK8s!"
echo ""

# Verify imported images
echo "🔍 Verifying imported images in MicroK8s:"
for service in "${!SERVICES[@]}"; do
    image="${SERVICES[$service]}"
    image_name="${image##*/}"
    if microk8s ctr images ls | grep -q "$image_name"; then
        echo "   ✅ $image_name (imported successfully)"
    else
        echo "   ❌ $image_name (not found)"
    fi
done

echo ""
echo "📋 Next Steps:"
echo "   1. Deploy to MicroK8s: cd ../k8s && ./deploy.sh"
echo "   2. Check pod status: microk8s kubectl get pods -n eglobalshop"
echo "   3. View services: microk8s kubectl get svc -n eglobalshop"
echo ""
echo "💡 Useful MicroK8s commands:"
echo "   microk8s kubectl get nodes"
echo "   microk8s kubectl get pods --all-namespaces"
echo "   microk8s ctr images ls | grep eglobalshop"
echo ""
echo "🔧 Troubleshooting:"
echo "   - If pods are stuck: microk8s kubectl describe pod <pod-name> -n eglobalshop"
echo "   - Delete deployment: microk8s kubectl delete namespace eglobalshop"
echo "   - Restart MicroK8s: microk8s stop && microk8s start"