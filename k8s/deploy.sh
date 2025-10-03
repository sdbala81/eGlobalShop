#!/bin/bash

# eGlobalShop Kubernetes Deployment Script
# This script deploys all eGlobalShop microservices to a Kubernetes cluster in dependency order
# Usage: ./deploy.sh

set -e

echo "🚀 Deploying eGlobalShop to Kubernetes..."
echo "📋 Deployment will proceed in dependency order:"
echo "   1. Namespace"
echo "   2. Infrastructure Services (PostgreSQL, NATS, Seq, PgAdmin)"
echo "   3. Application Services (Orders, Customers, Inventory, Billing, Shipping)"
echo ""

# Navigate to the directory containing this script
cd "$(dirname "$0")"

# Step 1: Create namespace first
echo "📦 Step 1: Creating namespace..."
kubectl apply -f namespace.yaml
echo "✅ Namespace created successfully"
echo ""

# Step 2: Deploy core infrastructure services (PostgreSQL first as it's required by all apps)
echo "🔧 Step 2: Deploying core infrastructure services..."

# Deploy PostgreSQL first (required by all microservices)
echo "  📄 Deploying PostgreSQL (required by all microservices)..."
kubectl apply -f postgres/

echo "⏳ Waiting for PostgreSQL to be ready (this is critical for app services)..."
kubectl wait --for=condition=available --timeout=300s deployment/postgres-db -n eglobalshop || {
    echo "❌ PostgreSQL deployment failed or timed out. Cannot proceed with application services."
    echo "💡 Check PostgreSQL status with: kubectl get pods -n eglobalshop -l app=postgres-db"
    exit 1
}
echo "✅ PostgreSQL is ready"

# Deploy NATS (required by all microservices for messaging)
echo "  📄 Deploying NATS (required by all microservices)..."
kubectl apply -f nats/

echo "⏳ Waiting for NATS to be ready..."
kubectl wait --for=condition=available --timeout=120s deployment/nats-server -n eglobalshop || {
    echo "❌ NATS deployment failed or timed out. Cannot proceed with application services."
    echo "💡 Check NATS status with: kubectl get pods -n eglobalshop -l app=nats-server"
    exit 1
}
echo "✅ NATS is ready"

# Deploy supporting infrastructure (Seq and PgAdmin - non-critical for app functionality)
echo "  📄 Deploying supporting services (Seq, PgAdmin)..."
kubectl apply -f seq/
kubectl apply -f pgadmin/

echo "⏳ Waiting for supporting services (non-blocking)..."
kubectl wait --for=condition=available --timeout=60s deployment/seq -n eglobalshop || echo "⚠️  Seq deployment timeout - continuing (non-critical)"
kubectl wait --for=condition=available --timeout=60s deployment/pgadmin -n eglobalshop || echo "⚠️  PgAdmin deployment timeout - continuing (non-critical)"

echo "✅ Infrastructure services deployed"
echo ""

# Step 3: Deploy application microservices (now that dependencies are ready)
echo "🏗️ Step 3: Deploying application microservices..."
echo "  All microservices can be deployed in parallel since dependencies are ready"

# Deploy all microservices simultaneously since dependencies are now ready
echo "  📄 Deploying all microservices..."

# Deploy all microservices in parallel using background processes
kubectl apply -f customers/ &
kubectl apply -f orders/ &
kubectl apply -f inventory/ &
kubectl apply -f billing/ &
kubectl apply -f shipping/ &



echo "✅ All application services deployed"

echo ""
echo "🎉 Deployment completed successfully!"
echo ""

# Step 4: Show final deployment status and service information
echo "📊 Final Deployment Status:"
kubectl get all -n eglobalshop -o wide
echo ""


echo "🌍 External Access URLs (via NodePort):"
echo "   Application Services:"
echo "   ├── Orders Service:     http://<node-ip>:30100"
echo "   ├── Inventory Service:  http://<node-ip>:30200"
echo "   ├── Customers Service:  http://<node-ip>:30300"
echo "   ├── Shipping Service:   http://<node-ip>:30400"
echo "   └── Billing Service:    http://<node-ip>:30500"
echo ""
echo "   Infrastructure Services:"
echo "   ├── PgAdmin:            http://<node-ip>:30041"
echo "   ├── NATS Monitor:       http://<node-ip>:30042"
echo "   └── Seq Logs:           http://<node-ip>:30040"
echo ""

# Get node IP for convenience
NODE_IP=$(kubectl get nodes -o jsonpath='{.items[0].status.addresses[?(@.type=="InternalIP")].address}' 2>/dev/null || echo "<node-ip>")
if [ "$NODE_IP" != "<node-ip>" ]; then
    echo "💡 Your cluster node IP: $NODE_IP"
    echo "   Example: http://$NODE_IP:30100 (Orders Service)"
    echo ""
fi

echo "🔍 Useful Commands:"
echo "   Check all pods:         kubectl get pods -n eglobalshop"
echo "   Check services:         kubectl get services -n eglobalshop"
echo "   View logs:              kubectl logs deployment/<service-name> -n eglobalshop"
echo "   Follow logs:            kubectl logs -f deployment/<service-name> -n eglobalshop"
echo "   Describe pod:           kubectl describe pod <pod-name> -n eglobalshop"
echo ""
echo "� To clean up all resources:"
echo "   kubectl delete namespace eglobalshop"
echo ""
echo "🚀 eGlobalShop is now running on Kubernetes!"