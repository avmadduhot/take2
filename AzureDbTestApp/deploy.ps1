# Simple Application Build Script for Manual Azure Deployment
# This script builds and packages the application for manual upload to Azure App Service

# Configuration - UPDATE THESE VALUES FOR YOUR AZURE SQL DATABASE
# this doesn't actually matter, it's just being used in the print statement
$SQL_SERVER = "amaddula-test1-server"
$SQL_DATABASE = "amaddula-test1-database"
$SQL_USERNAME = "amaddula-test1-server-admin"
$SQL_PASSWORD = "Test"

Write-Host "Building application for manual Azure deployment..." -ForegroundColor Green

# Build and package application
Write-Host "Building and packaging application..." -ForegroundColor Yellow
dotnet publish -c Release -o ./publish
Compress-Archive -Path ./publish/* -DestinationPath ./app.zip -Force

# Clean up publish folder
Remove-Item ./publish -Recurse -Force

# Display connection string for Azure App Service configuration
Write-Host "Build completed!" -ForegroundColor Green
Write-Host "Package created: app.zip" -ForegroundColor Cyan
Write-Host ""
Write-Host "Manual Deployment Steps:" -ForegroundColor Yellow
Write-Host "1. Upload 'app.zip' to your Azure App Service" -ForegroundColor White
Write-Host "2. Configure the following connection string in Azure App Service:" -ForegroundColor White
$CONNECTION_STRING = "Server=tcp:$SQL_SERVER.database.windows.net,1433;Initial Catalog=$SQL_DATABASE;Persist Security Info=False;User ID=$SQL_USERNAME;Password=$SQL_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
Write-Host "   Connection String Name: DefaultConnection" -ForegroundColor Cyan
Write-Host "   Connection String Value: $CONNECTION_STRING" -ForegroundColor Cyan
Write-Host "   Connection String Type: SQLAzure" -ForegroundColor Cyan
Write-Host ""
Write-Host "3. Test your application at: https://YOUR_APP_NAME.azurewebsites.net/DatabaseTest" -ForegroundColor White
Write-Host ""
Write-Host "Note: Make sure your SQL Database server allows Azure services to access it." -ForegroundColor Yellow
