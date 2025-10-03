#!/bin/bash

# Script to delete Docker Hub repositories
# This script reads credentials from .env file and deletes specified repositories

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if .env file exists
if [ ! -f "../.env" ]; then
    print_error ".env file not found in parent directory"
    exit 1
fi

# Source the .env file
source "../.env"

# Check if required variables are set
if [ -z "$CONTAINER_REGISTRY_USERNAME" ] || [ -z "$CONTAINER_REGISTRY_PASSWORD" ]; then
    print_error "CONTAINER_REGISTRY_USERNAME and CONTAINER_REGISTRY_PASSWORD must be set in .env file"
    exit 1
fi

# Docker Hub repositories to delete
repositories=(
    "sdbala/eglobalshop-order-service"
    "sdbala/eglobalshop-inventory-service"
    "sdbala/eglobalshop-payment-service"
    "sdbala/eglobalshop-customer-service"
    "sdbala/eglobalshop-shipping-service"
    "sdbala/order-service"
    "sdbala/inventory-service"
    "sdbala/payment-service"
    "sdbala/customer-service"
    "sdbala/shipping-service"
)

# Function to get Docker Hub token
get_docker_hub_token() {
    local response
    response=$(curl -s -X POST \
        -H "Content-Type: application/json" \
        -d "{\"username\": \"$CONTAINER_REGISTRY_USERNAME\", \"password\": \"$CONTAINER_REGISTRY_PASSWORD\"}" \
        "https://hub.docker.com/v2/users/login/")
    
    if [ $? -ne 0 ]; then
        print_error "Failed to authenticate with Docker Hub"
        return 1
    fi
    
    echo "$response" | grep -o '"token":"[^"]*"' | cut -d'"' -f4
}

# Function to delete repository
delete_repository() {
    local repo="$1"
    local token="$2"
    
    print_info "Deleting repository: $repo"
    
    local response
    response=$(curl -s -w "%{http_code}" -X DELETE \
        -H "Authorization: JWT $token" \
        "https://hub.docker.com/v2/repositories/$repo/")
    
    local http_code="${response: -3}"
    
    case $http_code in
        204)
            print_info "Successfully deleted: $repo"
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

# Main execution
print_info "Starting Docker Hub repository cleanup..."
print_info "Username: $CONTAINER_REGISTRY_USERNAME"

# Get authentication token
print_info "Authenticating with Docker Hub..."
TOKEN=$(get_docker_hub_token)

if [ -z "$TOKEN" ]; then
    print_error "Failed to get authentication token"
    exit 1
fi

print_info "Authentication successful"

# Prompt for confirmation
echo
print_warning "This will delete the following repositories:"
for repo in "${repositories[@]}"; do
    echo "  - $repo"
done
echo

read -p "Are you sure you want to proceed? (y/N): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    print_info "Operation cancelled"
    exit 0
fi

# Delete repositories
failed_deletions=()
successful_deletions=()

for repo in "${repositories[@]}"; do
    if delete_repository "$repo" "$TOKEN"; then
        successful_deletions+=("$repo")
    else
        failed_deletions+=("$repo")
    fi
    sleep 1  # Rate limiting - be nice to Docker Hub API
done

# Summary
echo
print_info "=== DELETION SUMMARY ==="
print_info "Successfully deleted: ${#successful_deletions[@]} repositories"
for repo in "${successful_deletions[@]}"; do
    echo "  ✓ $repo"
done

if [ ${#failed_deletions[@]} -gt 0 ]; then
    print_error "Failed to delete: ${#failed_deletions[@]} repositories"
    for repo in "${failed_deletions[@]}"; do
        echo "  ✗ $repo"
    done
    exit 1
else
    print_info "All repositories deleted successfully!"
fi