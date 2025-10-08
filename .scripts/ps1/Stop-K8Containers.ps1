# eGlobalShop Kubernetes Cleanup Script
# This script removes all eGlobalShop resources from the Kubernetes cluster
# Usage: .\Stop-K8Containers.ps1

$ErrorActionPreference = "Stop"

Write-Host "🛑 Stopping and removing all eGlobalShop resources from Kubernetes..." -ForegroundColor Yellow
Write-Host "⚠️  This will permanently delete all resources in the eglobalshop namespace!" -ForegroundColor Yellow
Write-Host ""
Write-Host "📋 Cleanup will proceed in reverse dependency order:" -ForegroundColor Cyan
Write-Host "   1. Application Services (Orders, Customers, Inventory, Billing, Shipping)"
Write-Host "   2. Infrastructure Services (Seq, PgAdmin, NATS, PostgreSQL)"
Write-Host "   3. Namespace (this will remove any remaining resources)"
Write-Host ""

# Store the original location and navigate to the Kubernetes manifests directory
$originalLocation = Get-Location
Set-Location (Join-Path $PSScriptRoot "..\..\.k8s")

# Function to check if namespace exists
function Test-NamespaceExists {
    try {
        kubectl get namespace eglobalshop *>&1 | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

# Check if the namespace exists
if (-not (Test-NamespaceExists)) {
    Write-Host "ℹ️  Namespace 'eglobalshop' does not exist. Nothing to clean up." -ForegroundColor Blue
    Set-Location $originalLocation
    exit 0
}

Write-Host "📊 Current resources in eglobalshop namespace:" -ForegroundColor Cyan
try {
    kubectl get all -n eglobalshop 2>$null
}
catch {
    Write-Host "No resources found"
}
Write-Host ""

# Skip confirmation - proceed directly to cleanup
Write-Host "🧹 Starting cleanup process..." -ForegroundColor Green
Write-Host ""

# Step 1: Scale down all deployments gracefully (faster shutdown)
Write-Host "⬇️  Step 1: Scaling down all deployments..." -ForegroundColor Yellow
try {
    $deployments = kubectl get deployments -n eglobalshop --no-headers -o custom-columns=":metadata.name" 2>$null
    if ($deployments) {
        $deployments | ForEach-Object {
            if ($_.Trim()) {
                Write-Host "  📉 Scaling down deployment: $_" -ForegroundColor Gray
                try {
                    kubectl scale deployment $_.Trim() --replicas=0 -n eglobalshop 2>$null
                }
                catch {
                    Write-Host "    ⚠️  Failed to scale $_" -ForegroundColor Yellow
                }
            }
        }
    }
}
catch {
    Write-Host "  No deployments found to scale down" -ForegroundColor Gray
}

# Wait a moment for graceful shutdown
Write-Host "⏳ Waiting for pods to terminate gracefully..." -ForegroundColor Blue
Start-Sleep -Seconds 5

# Step 2: Delete all application services (in reverse dependency order)
Write-Host ""
Write-Host "🗑️  Step 2: Removing application services..." -ForegroundColor Yellow

# Remove microservices first
$services = @("shipping", "billing", "inventory", "orders", "customers")
foreach ($service in $services) {
    if (Test-Path $service) {
        Write-Host "  🗑️  Removing $service service..." -ForegroundColor Gray
        try {
            kubectl delete -f "$service/" --ignore-not-found=true 2>$null
        }
        catch {
            Write-Host "    ⚠️  Some $service resources may not exist" -ForegroundColor Yellow
        }
    }
}

Write-Host ""
Write-Host "🗑️  Step 3: Removing infrastructure services..." -ForegroundColor Yellow

# Remove supporting infrastructure first
$infraServices = @("pgadmin", "seq")
foreach ($service in $infraServices) {
    if (Test-Path $service) {
        Write-Host "  🗑️  Removing $service..." -ForegroundColor Gray
        try {
            kubectl delete -f "$service/" --ignore-not-found=true 2>$null
        }
        catch {
            Write-Host "    ⚠️  Some $service resources may not exist" -ForegroundColor Yellow
        }
    }
}

# Remove core infrastructure (NATS, then PostgreSQL last as it has data)
Write-Host "  🗑️  Removing NATS..." -ForegroundColor Gray
try {
    kubectl delete -f nats/ --ignore-not-found=true 2>$null
}
catch {
    Write-Host "    ⚠️  Some NATS resources may not exist" -ForegroundColor Yellow
}

Write-Host "  🗑️  Removing PostgreSQL (this may take a moment due to persistent data)..." -ForegroundColor Gray
try {
    kubectl delete -f postgres/ --ignore-not-found=true 2>$null
}
catch {
    Write-Host "    ⚠️  Some PostgreSQL resources may not exist" -ForegroundColor Yellow
}

# Step 4: Wait for pods to terminate gracefully
Write-Host ""
Write-Host "⏳ Waiting for pods to terminate gracefully..." -ForegroundColor Blue
try {
    kubectl wait --for=delete pods --all -n eglobalshop --timeout=120s 2>$null
}
catch {
    Write-Host "⚠️  Some pods may still be terminating - continuing with namespace deletion" -ForegroundColor Yellow
}

# Step 5: Force delete any remaining resources in the namespace
Write-Host ""
Write-Host "🧹 Step 4: Force cleaning any remaining resources..." -ForegroundColor Green

# Delete any remaining resources that might not have been covered by the manifests
$resourceTypes = @("deployments", "services", "configmaps", "secrets", "persistentvolumeclaims", "pods", "replicasets")
foreach ($resource in $resourceTypes) {
    try {
        $count = (kubectl get $resource -n eglobalshop --no-headers 2>$null | Measure-Object).Count
        if ($count -gt 0) {
            Write-Host "  🗑️  Force deleting remaining $resource..." -ForegroundColor Gray
            try {
                kubectl delete $resource --all -n eglobalshop --force --grace-period=0 2>$null
            }
            catch {
                Write-Host "    ⚠️  Failed to delete some $resource" -ForegroundColor Yellow
            }
        }
    }
    catch {
        # Resource type may not exist, continue
    }
}

# Step 6: Delete the namespace itself
Write-Host ""
Write-Host "🗑️  Step 5: Removing the eglobalshop namespace..." -ForegroundColor Yellow
kubectl delete namespace eglobalshop --ignore-not-found=true

# Wait for namespace deletion to complete
Write-Host "⏳ Waiting for namespace deletion to complete..." -ForegroundColor Blue
$timeout = 60
while ((Test-NamespaceExists) -and ($timeout -gt 0)) {
    Write-Host "  ⏳ Namespace still exists, waiting... ($timeout seconds remaining)" -ForegroundColor Gray
    Start-Sleep -Seconds 5
    $timeout -= 5
}

if (Test-NamespaceExists) {
    Write-Host "⚠️  Namespace deletion is taking longer than expected. This is normal for namespaces with persistent volumes." -ForegroundColor Yellow
    Write-Host "💡 You can check the status with: kubectl get namespace eglobalshop" -ForegroundColor Cyan
}
else {
    Write-Host "✅ Namespace deleted successfully" -ForegroundColor Green
}

Write-Host ""
Write-Host "🎉 Cleanup completed!" -ForegroundColor Green
Write-Host ""

# Verify cleanup
Write-Host "📊 Verification - Checking for any remaining eGlobalShop resources..." -ForegroundColor Cyan
if (Test-NamespaceExists) {
    Write-Host "⚠️  Namespace still exists (may be in 'Terminating' state):" -ForegroundColor Yellow
    kubectl get namespace eglobalshop
    Write-Host ""
    Write-Host "💡 If namespace is stuck in 'Terminating' state, you may need to:" -ForegroundColor Cyan
    Write-Host "   1. Check for finalizers: kubectl get namespace eglobalshop -o yaml"
    Write-Host "   2. Force delete if needed: kubectl delete namespace eglobalshop --force --grace-period=0"
}
else {
    Write-Host "✅ No eGlobalShop namespace found - cleanup successful!" -ForegroundColor Green
}

Write-Host ""
Write-Host "🔍 Useful commands for verification:" -ForegroundColor Cyan
Write-Host "   Check all namespaces:   kubectl get namespaces"
Write-Host "   Check persistent volumes: kubectl get pv | Select-String eglobalshop"
Write-Host "   Check if any pods remain: kubectl get pods --all-namespaces | Select-String eglobalshop"
Write-Host ""

# Check for any remaining PVCs or PVs that might need manual cleanup
try {
    $remainingPVs = kubectl get pv -o jsonpath='{.items[?(@.spec.claimRef.namespace=="eglobalshop")].metadata.name}' 2>$null
    if ($remainingPVs -and $remainingPVs.Trim()) {
        Write-Host "⚠️  WARNING: Found persistent volumes that may need manual cleanup:" -ForegroundColor Yellow
        $remainingPVs.Split(' ') | Where-Object { $_.Trim() } | ForEach-Object {
            Write-Host "   - $_"
        }
        Write-Host ""
        Write-Host "💡 To remove these manually (WARNING - this will delete data):" -ForegroundColor Cyan
        Write-Host "   kubectl delete pv $remainingPVs"
        Write-Host ""
    }
}
catch {
    # No persistent volumes found or error checking - continue
}

Write-Host "🛑 eGlobalShop has been stopped and removed from Kubernetes!" -ForegroundColor Green

# Return to the original location
Set-Location $originalLocation