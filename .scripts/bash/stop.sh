#!/bin/bash

# eGlobalShop Kubernetes Cleanup Script
# This script removes all eGlobalShop resources from the Kubernetes cluster
# Usage: ./stop.sh

set -e

echo "🛑 Stopping and removing all eGlobalShop resources from Kubernetes..."
echo "⚠️  This will permanently delete all resources in the eglobalshop namespace!"
echo ""
echo "📋 Cleanup will proceed in reverse dependency order:"
echo "   1. Application Services (Orders, Customers, Inventory, Billing, Shipping)"
echo "   2. Infrastructure Services (Seq, PgAdmin, NATS, PostgreSQL)"
echo "   3. Namespace (this will remove any remaining resources)"
echo ""

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Function to check if namespace exists
check_namespace() {
    if kubectl get namespace eglobalshop &> /dev/null; then
        return 0
    else
        return 1
    fi
}

# Check if the namespace exists
if ! check_namespace; then
    echo "ℹ️  Namespace 'eglobalshop' does not exist. Nothing to clean up."
    exit 0
fi

echo "📊 Current resources in eglobalshop namespace:"
kubectl get all -n eglobalshop 2>/dev/null || echo "No resources found"
echo ""

# Skip confirmation - proceed directly to cleanup
echo "🧹 Starting cleanup process..."
echo ""

# Step 1: Scale down all deployments gracefully (faster shutdown)
echo "⬇️  Step 1: Scaling down all deployments..."
kubectl get deployments -n eglobalshop --no-headers -o custom-columns=":metadata.name" 2>/dev/null | while read deployment; do
    if [ -n "$deployment" ]; then
        echo "  📉 Scaling down deployment: $deployment"
        kubectl scale deployment "$deployment" --replicas=0 -n eglobalshop 2>/dev/null || echo "    ⚠️  Failed to scale $deployment"
    fi
done

# Wait a moment for graceful shutdown
echo "⏳ Waiting for pods to terminate gracefully..."
sleep 5

# Step 2: Delete all application services (in reverse dependency order)
echo ""
echo "🗑️  Step 2: Removing application services..."

# Remove microservices first
services=("shipping" "billing" "inventory" "orders" "customers")
for service in "${services[@]}"; do
    if [ -d "$service" ]; then
        echo "  🗑️  Removing $service service..."
        kubectl delete -f "$service/" --ignore-not-found=true 2>/dev/null || echo "    ⚠️  Some $service resources may not exist"
    fi
done

echo ""
echo "🗑️  Step 3: Removing infrastructure services..."

# Remove supporting infrastructure first
infra_services=("pgadmin" "seq")
for service in "${infra_services[@]}"; do
    if [ -d "$service" ]; then
        echo "  🗑️  Removing $service..."
        kubectl delete -f "$service/" --ignore-not-found=true 2>/dev/null || echo "    ⚠️  Some $service resources may not exist"
    fi
done

# Remove core infrastructure (NATS, then PostgreSQL last as it has data)
echo "  🗑️  Removing NATS..."
kubectl delete -f nats/ --ignore-not-found=true 2>/dev/null || echo "    ⚠️  Some NATS resources may not exist"

echo "  🗑️  Removing PostgreSQL (this may take a moment due to persistent data)..."
kubectl delete -f postgres/ --ignore-not-found=true 2>/dev/null || echo "    ⚠️  Some PostgreSQL resources may not exist"

# Step 4: Wait for pods to terminate gracefully
echo ""
echo "⏳ Waiting for pods to terminate gracefully..."
kubectl wait --for=delete pods --all -n eglobalshop --timeout=120s || echo "⚠️  Some pods may still be terminating - continuing with namespace deletion"

# Step 5: Force delete any remaining resources in the namespace
echo ""
echo "🧹 Step 4: Force cleaning any remaining resources..."

# Delete any remaining resources that might not have been covered by the manifests
resource_types=("deployments" "services" "configmaps" "secrets" "persistentvolumeclaims" "pods" "replicasets")
for resource in "${resource_types[@]}"; do
    count=$(kubectl get "$resource" -n eglobalshop --no-headers 2>/dev/null | wc -l)
    if [ "$count" -gt 0 ]; then
        echo "  🗑️  Force deleting remaining $resource..."
        kubectl delete "$resource" --all -n eglobalshop --force --grace-period=0 2>/dev/null || echo "    ⚠️  Failed to delete some $resource"
    fi
done

# Step 6: Delete the namespace itself
echo ""
echo "🗑️  Step 5: Removing the eglobalshop namespace..."
kubectl delete namespace eglobalshop --ignore-not-found=true

# Wait for namespace deletion to complete
echo "⏳ Waiting for namespace deletion to complete..."
timeout=60
while kubectl get namespace eglobalshop &> /dev/null && [ $timeout -gt 0 ]; do
    echo "  ⏳ Namespace still exists, waiting... ($timeout seconds remaining)"
    sleep 5
    timeout=$((timeout - 5))
done

if kubectl get namespace eglobalshop &> /dev/null; then
    echo "⚠️  Namespace deletion is taking longer than expected. This is normal for namespaces with persistent volumes."
    echo "💡 You can check the status with: kubectl get namespace eglobalshop"
else
    echo "✅ Namespace deleted successfully"
fi

echo ""
echo "🎉 Cleanup completed!"
echo ""

# Verify cleanup
echo "📊 Verification - Checking for any remaining eGlobalShop resources..."
if kubectl get namespace eglobalshop &> /dev/null; then
    echo "⚠️  Namespace still exists (may be in 'Terminating' state):"
    kubectl get namespace eglobalshop
    echo ""
    echo "💡 If namespace is stuck in 'Terminating' state, you may need to:"
    echo "   1. Check for finalizers: kubectl get namespace eglobalshop -o yaml"
    echo "   2. Force delete if needed: kubectl delete namespace eglobalshop --force --grace-period=0"
else
    echo "✅ No eGlobalShop namespace found - cleanup successful!"
fi

echo ""
echo "🔍 Useful commands for verification:"
echo "   Check all namespaces:   kubectl get namespaces"
echo "   Check persistent volumes: kubectl get pv | grep eglobalshop"
echo "   Check if any pods remain: kubectl get pods --all-namespaces | grep eglobalshop"
echo ""

# Check for any remaining PVCs or PVs that might need manual cleanup
REMAINING_PVS=$(kubectl get pv -o jsonpath='{.items[?(@.spec.claimRef.namespace=="eglobalshop")].metadata.name}' 2>/dev/null || echo "")
if [ -n "$REMAINING_PVS" ]; then
    echo "⚠️  WARNING: Found persistent volumes that may need manual cleanup:"
    echo "$REMAINING_PVS" | tr ' ' '\n' | sed 's/^/   - /'
    echo ""
    echo "💡 To remove these manually (WARNING - this will delete data):"
    echo "   kubectl delete pv $REMAINING_PVS"
    echo ""
fi

echo "🛑 eGlobalShop has been stopped and removed from Kubernetes!"