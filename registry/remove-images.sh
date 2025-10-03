#!/bin/bash

# Script to remove eGlobalShop images both locally and from Docker Hub
# This script combines local image cleanup and Docker Hub repository deletion

# Removed 'set -e' to allow script to continue on individual failures

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

print_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Docker Hub repositories to delete
repositories=(
    "sdbala/eglobalshop-order-service"
    "sdbala/eglobalshop-inventory-service"
    "sdbala/eglobalshop-payment-service"
    "sdbala/eglobalshop-customer-service"
    "sdbala/eglobalshop-shipping-service"
)

# Function to remove local images
remove_local_images() {
    print_info "=== Removing Local Images ==="
    
    # Remove local eglobalshop images
    echo "Checking for eglobalshop images..."
    image_ids=$(podman images --filter=reference="*eglobalshop*" -q 2>/dev/null || true)
    
    if [ -n "$image_ids" ]; then
        echo "Found eglobalshop images:"
        podman images --filter=reference="*eglobalshop*" --format "table {{.Repository}}:{{.Tag}} {{.ID}}" 2>/dev/null || true
        echo "Removing these images..."
        
        # Remove images one by one to continue even if some fail
        local removed_count=0
        local failed_count=0
        while IFS= read -r image_id; do
            if [ -n "$image_id" ]; then
                if podman rmi -f "$image_id" 2>/dev/null; then
                    ((removed_count++))
                else
                    print_warning "Failed to remove image: $image_id"
                    ((failed_count++))
                fi
            fi
        done <<< "$image_ids"
        
        print_info "✓ Removed $removed_count local eglobalshop images ($failed_count failed)"
    else
        print_info "No local eglobalshop images found"
    fi
    
    # Clean up dangling images
    echo "Cleaning up dangling images..."
    dangling_images=$(podman images -f "dangling=true" -q 2>/dev/null || true)
    if [ -n "$dangling_images" ]; then
        local dangling_removed=0
        local dangling_failed=0
        while IFS= read -r image_id; do
            if [ -n "$image_id" ]; then
                if podman rmi -f "$image_id" 2>/dev/null; then
                    ((dangling_removed++))
                else
                    ((dangling_failed++))
                fi
            fi
        done <<< "$dangling_images"
        print_info "✓ Removed $dangling_removed dangling images ($dangling_failed failed)"
    else
        print_info "No dangling images found"
    fi
    echo ""
}

# Function to get Docker Hub token
get_docker_hub_token() {
    local response
    response=$(curl -s -X POST \
        -H "Content-Type: application/json" \
        -d "{\"username\": \"$CONTAINER_REGISTRY_USERNAME\", \"password\": \"$CONTAINER_REGISTRY_PASSWORD\"}" \
        "https://hub.docker.com/v2/users/login/" 2>/dev/null)
    
    if [ $? -ne 0 ] || [ -z "$response" ]; then
        return 1
    fi
    
    echo "$response" | grep -o '"token":"[^"]*"' | cut -d'"' -f4
}

# Function to delete Docker Hub repository
delete_repository() {
    local repo="$1"
    local token="$2"
    
    print_info "Deleting repository: $repo"
    
    local response
    response=$(curl -s -w "%{http_code}" -X DELETE \
        -H "Authorization: JWT $token" \
        "https://hub.docker.com/v2/repositories/$repo/" 2>/dev/null)
    
    local http_code="${response: -3}"
    
    case $http_code in
        2[0-9][0-9])
            print_info "✓ Successfully deleted: $repo (HTTP: $http_code)"
            return 0
            ;;
        404)
            print_warning "Repository not found (may already be deleted): $repo"
            return 0
            ;;
        401)
            print_error "Authentication failed for: $repo"
            return 1
            ;;
        403)
            print_error "Access denied for: $repo"
            return 1
            ;;
        *)
            print_error "Failed to delete $repo (HTTP: $http_code)"
            return 1
            ;;
    esac
}

# Function to remove Docker Hub repositories
remove_docker_hub_repositories() {
    print_info "=== Removing Docker Hub Repositories ==="
    
    # Check if .env file exists
    if [ ! -f "../.env" ]; then
        print_warning ".env file not found - skipping Docker Hub cleanup"
        return 0
    fi
    
    # Source the .env file
    source "../.env" 2>/dev/null || {
        print_warning "Failed to read .env file - skipping Docker Hub cleanup"
        return 0
    }
    
    # Check if required variables are set
    if [ -z "$CONTAINER_REGISTRY_USERNAME" ] || [ -z "$CONTAINER_REGISTRY_PASSWORD" ]; then
        print_warning "Docker Hub credentials not found in .env - skipping Docker Hub cleanup"
        return 0
    fi
    
    print_info "Authenticating with Docker Hub (user: $CONTAINER_REGISTRY_USERNAME)..."
    TOKEN=$(get_docker_hub_token)
    
    if [ -z "$TOKEN" ]; then
        print_warning "Failed to get authentication token - skipping Docker Hub cleanup"
        return 0
    fi
    
    print_info "Authentication successful"
    
    # Delete repositories - continue even if some fail
    local failed_count=0
    local success_count=0
    
    for repo in "${repositories[@]}"; do
        # Continue processing even if individual repository deletion fails
        if delete_repository "$repo" "$TOKEN" 2>/dev/null; then
            ((success_count++))
        else
            print_warning "Failed to delete repository: $repo (continuing...)"
            ((failed_count++))
        fi
        sleep 1  # Rate limiting
    done
    
    print_info "Docker Hub cleanup complete: $success_count successful, $failed_count failed"
    echo ""
}

# Main execution
print_info "Starting eGlobalShop image cleanup..."

# Prompt for confirmation
echo
print_warning "This will:"
echo "  - Remove all local eglobalshop images"
echo "  - Delete Docker Hub repositories (if credentials available)"
echo

read -p "Are you sure you want to proceed? (y/N): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    print_info "Operation cancelled"
    exit 0
fi

# Remove local images
remove_local_images

# Remove Docker Hub repositories
remove_docker_hub_repositories

print_info "=== Cleanup Complete ==="
print_info "Remaining local images:"
podman images --format "table {{.Repository}}:{{.Tag}} {{.ID}} {{.Size}}"