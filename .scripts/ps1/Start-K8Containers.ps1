# eGlobalShop Kubernetes Deployment Script (PowerShell)
# This script deploys all eGlobalShop microservices to a Kubernetes cluster in dependency order
# Usage: .\Start-K8Containers.ps1

$ErrorActionPreference = "Stop"

Write-Host "🚀 Deploying eGlobalShop to Kubernetes..." -ForegroundColor Green
Write-Host "📋 Deployment will proceed in dependency order:" -ForegroundColor Cyan
Write-Host "   1. Namespace"
Write-Host "   2. Infrastructure Services (PostgreSQL, NATS, Seq, PgAdmin)"
Write-Host "   3. Application Services (Orders, Customers, Inventory, Billing, Shipping)"
Write-Host ""

# Store the original location and navigate to the Kubernetes manifests directory
$originalLocation = Get-Location
Set-Location (Join-Path $PSScriptRoot "..\..\.k8s")

# Step 1: Create namespace first
Write-Host "📦 Step 1: Creating namespace..." -ForegroundColor Yellow
kubectl apply -f namespace.yaml
Write-Host "✅ Namespace created successfully" -ForegroundColor Green
Write-Host ""

# Step 2: Deploy core infrastructure services (PostgreSQL first as it's required by all apps)
Write-Host "🔧 Step 2: Deploying core infrastructure services..." -ForegroundColor Yellow

# Deploy PostgreSQL first (required by all microservices)
Write-Host "  📄 Deploying PostgreSQL (required by all microservices)..." -ForegroundColor Cyan
kubectl apply -f postgres/

Write-Host "⏳ Waiting for PostgreSQL to be ready (this is critical for app services)..." -ForegroundColor Yellow
$postgresResult = kubectl wait --for=condition=available --timeout=300s deployment/postgres-db -n eglobalshop 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ PostgreSQL deployment failed or timed out. Cannot proceed with application services." -ForegroundColor Red
    Write-Host "💡 Check PostgreSQL status with: kubectl get pods -n eglobalshop -l app=postgres-db" -ForegroundColor Yellow
    Set-Location $originalLocation
    exit 1
}
Write-Host "✅ PostgreSQL is ready" -ForegroundColor Green

# Deploy NATS (required by all microservices for messaging)
Write-Host "  📄 Deploying NATS (required by all microservices)..." -ForegroundColor Cyan
kubectl apply -f nats/

Write-Host "⏳ Waiting for NATS to be ready..." -ForegroundColor Yellow
$natsResult = kubectl wait --for=condition=available --timeout=120s deployment/nats-server -n eglobalshop 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ NATS deployment failed or timed out. Cannot proceed with application services." -ForegroundColor Red
    Write-Host "💡 Check NATS status with: kubectl get pods -n eglobalshop -l app=nats-server" -ForegroundColor Yellow
    Set-Location $originalLocation
    exit 1
}
Write-Host "✅ NATS is ready" -ForegroundColor Green

# Deploy supporting infrastructure (Seq and PgAdmin - non-critical for app functionality)
Write-Host "  📄 Deploying supporting services (Seq, PgAdmin)..." -ForegroundColor Cyan
kubectl apply -f seq/
kubectl apply -f pgadmin/

Write-Host "⏳ Waiting for supporting services (non-blocking)..." -ForegroundColor Yellow
$seqResult = kubectl wait --for=condition=available --timeout=60s deployment/seq -n eglobalshop 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "⚠️  Seq deployment timeout - continuing (non-critical)" -ForegroundColor Yellow
}

$pgadminResult = kubectl wait --for=condition=available --timeout=60s deployment/pgadmin -n eglobalshop 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "⚠️  PgAdmin deployment timeout - continuing (non-critical)" -ForegroundColor Yellow
}

Write-Host "✅ Infrastructure services deployed" -ForegroundColor Green
Write-Host ""

# Step 3: Deploy application microservices (now that dependencies are ready)
Write-Host "🏗️ Step 3: Deploying application microservices..." -ForegroundColor Yellow
Write-Host "  All microservices can be deployed in parallel since dependencies are ready"

# Deploy all microservices simultaneously since dependencies are now ready
Write-Host "  📄 Deploying all microservices..." -ForegroundColor Cyan

# Deploy all microservices in parallel using background jobs
$jobs = @()
$jobs += Start-Job -ScriptBlock { kubectl apply -f customers/ }
$jobs += Start-Job -ScriptBlock { kubectl apply -f orders/ }
$jobs += Start-Job -ScriptBlock { kubectl apply -f inventory/ }
$jobs += Start-Job -ScriptBlock { kubectl apply -f billing/ }
$jobs += Start-Job -ScriptBlock { kubectl apply -f shipping/ }

# Wait for all deployment jobs to complete
$jobs | Wait-Job | Out-Null
$jobs | Remove-Job

Write-Host "✅ All application services deployed" -ForegroundColor Green

Write-Host ""
Write-Host "🎉 Deployment completed successfully!" -ForegroundColor Green
Write-Host ""

# Step 4: Show final deployment status and service information
Write-Host "📊 Final Deployment Status:" -ForegroundColor Cyan
kubectl get all -n eglobalshop -o wide
Write-Host ""

Write-Host "🌍 External Access URLs (via NodePort):" -ForegroundColor Cyan
Write-Host "   Application Services:"
Write-Host "   ├── Orders Service:     http://<node-ip>:30100"
Write-Host "   ├── Inventory Service:  http://<node-ip>:30200"
Write-Host "   ├── Customers Service:  http://<node-ip>:30300"
Write-Host "   ├── Shipping Service:   http://<node-ip>:30400"
Write-Host "   └── Billing Service:    http://<node-ip>:30500"
Write-Host ""
Write-Host "   Infrastructure Services:"
Write-Host "   ├── PgAdmin:            http://<node-ip>:30041"
Write-Host "   ├── NATS Monitor:       http://<node-ip>:30042"
Write-Host "   └── Seq Logs:           http://<node-ip>:30040"
Write-Host ""

# Get node IP for convenience
try {
    $nodeIp = (kubectl get nodes -o jsonpath='{.items[0].status.addresses[?(@.type=="InternalIP")].address}' 2>$null)
    if ($nodeIp -and $nodeIp -ne "") {
        Write-Host "💡 Your cluster node IP: $nodeIp" -ForegroundColor Yellow
        Write-Host "   Example: http://$nodeIp`:30100 (Orders Service)"
        Write-Host ""
    }
}
catch {
    # Ignore errors getting node IP
}

Write-Host "🔍 Useful Commands:" -ForegroundColor Cyan
Write-Host "   Check all pods:         kubectl get pods -n eglobalshop"
Write-Host "   Check services:         kubectl get services -n eglobalshop"
Write-Host "   View logs:              kubectl logs deployment/<service-name> -n eglobalshop"
Write-Host "   Follow logs:            kubectl logs -f deployment/<service-name> -n eglobalshop"
Write-Host "   Describe pod:           kubectl describe pod <pod-name> -n eglobalshop"
Write-Host ""
Write-Host "🧹 To clean up all resources:" -ForegroundColor Yellow
Write-Host "   kubectl delete namespace eglobalshop"
Write-Host ""
Write-Host "🚀 eGlobalShop is now running on Kubernetes!" -ForegroundColor Green

# Return to the original location
Set-Location $originalLocation