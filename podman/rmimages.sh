#!/bin/bash

# Script to remove all eglobalshop-related images using force
# Handles both local and docker.io images

echo "=== Removing eGlobalShop Images ==="

# Function to remove images with error handling
remove_images() {
    local filter_pattern="$1"
    local description="$2"
    
    echo "Checking for $description..."
    
    # Get image IDs matching the pattern
    image_ids=$(podman images --filter=reference="$filter_pattern" -q 2>/dev/null)
    
    if [ -n "$image_ids" ]; then
        echo "Found images matching '$filter_pattern':"
        podman images --filter=reference="$filter_pattern" --format "table {{.Repository}}:{{.Tag}} {{.ID}} {{.Created}}"
        echo "Removing these images..."
        echo "$image_ids" | xargs -r podman rmi -f
        echo "✓ Removed $description"
    else
        echo "No images found matching '$filter_pattern'"
    fi
    echo ""
}

# Remove local eglobalshop images (without registry prefix)
remove_images "eglobalshop-*" "local eglobalshop images"

# Remove docker.io eglobalshop images
remove_images "docker.io/*eglobalshop*" "docker.io eglobalshop images"

# Remove any other registry eglobalshop images (catch-all)
remove_images "*eglobalshop*" "any remaining eglobalshop images"

# Clean up dangling images
echo "Cleaning up dangling images..."
dangling_images=$(podman images -f "dangling=true" -q 2>/dev/null)
if [ -n "$dangling_images" ]; then
    echo "$dangling_images" | xargs -r podman rmi -f
    echo "✓ Removed dangling images"
else
    echo "No dangling images found"
fi

echo ""
echo "=== Image Cleanup Complete ==="
echo "Remaining images:"
podman images --format "table {{.Repository}}:{{.Tag}} {{.ID}} {{.Size}} {{.Created}}"