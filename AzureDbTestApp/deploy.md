# Manual Azure Deployment Guide

This guide shows how to manually deploy the .NET application to an existing Azure App Service.

## Prerequisites

- .NET 9.0 SDK installed
- Azure App Service (Linux) already created
- Azure SQL Database already created and configured

## Step 1: Update Database Configuration

Edit `deploy.ps1` and update these variables with your Azure SQL Database details:
```powershell
$SQL_SERVER = "myserver"        # Your SQL server name (without .database.windows.net)
$SQL_DATABASE = "mydatabase"    # Your database name
$SQL_USERNAME = "myusername"    # Your SQL username
$SQL_PASSWORD = "mypassword"    # Your SQL password
```

## Step 2: Build and Package Application

Run the build script:
```powershell
.\deploy.ps1
```

This will:
- Build the application for production
- Create `app.zip` file ready for deployment
- Display the connection string for Azure configuration

## Step 3: Upload to Azure App Service

### Option A: Azure Portal (Recommended) - Detailed Steps

1. **Go to Azure Portal** (portal.azure.com)
2. **Find your App Service**:
   - Search for "App Services" in the top search bar
   - Click on your App Service name
3. **Open Kudu (Advanced Tools)**:
   - In the left sidebar, scroll down to "Development Tools"
   - Click on "Advanced Tools"
   - Click the "Go →" button (this opens Kudu in a new tab)
4. **Deploy the ZIP file**:
   - In the Kudu interface, click on "Tools" in the top menu
   - Select "Zip Push Deploy"
   - You'll see a drag-and-drop area that says "Drag zip file here to deploy"
   - Drag your `app.zip` file from your project folder to this area
   - Wait for the deployment to complete (you'll see progress messages)

### Option B: Alternative Portal Method (Deployment Center)

1. **Go to your App Service** in Azure Portal
2. **Click "Deployment Center"** in the left sidebar
3. **Choose "Local Git"** or "External Git"
4. **Or use the "ZIP Deploy" option** if available
5. **Upload your `app.zip` file**

### Option C: Azure CLI (If you have Azure CLI installed)
```bash
az webapp deploy --resource-group YOUR_RESOURCE_GROUP --name YOUR_APP_NAME --src-path ./app.zip --type zip
```

### Option D: Visual Studio Code Extension
1. Install the "Azure App Service" extension in VS Code
2. Sign in to Azure through VS Code
3. Right-click on your `app.zip` file in the Explorer
4. Select "Deploy to Web App"
5. Choose your Azure App Service

### 🎯 **Can't find Advanced Tools/Kudu?**
If you can't find "Advanced Tools" in your App Service:
1. Make sure your App Service is running (not stopped)
2. Look under "Development Tools" in the left sidebar
3. Alternative: Go directly to `https://YOUR_APP_NAME.scm.azurewebsites.net/ZipPushDeploy`

## Step 4: Configure Connection String in Azure

1. Go to Azure Portal → Your App Service
2. Navigate to **Settings** → **Configuration**
3. Under **Connection strings**, click **New connection string**
4. Set:
   - **Name**: `DefaultConnection`
   - **Value**: The connection string displayed by the build script
   - **Type**: `SQLAzure`
5. Click **OK** and **Save**

## Step 5: Test Your Application

1. Navigate to `https://YOUR_APP_NAME.azurewebsites.net`
2. Go to `/DatabaseTest` page
3. Test database connection and CRUD operations

## Connection String Format

The connection string should look like:
```
Server=tcp:YOUR_SERVER.database.windows.net,1433;Initial Catalog=YOUR_DATABASE;Persist Security Info=False;User ID=YOUR_USERNAME;Password=YOUR_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

## Troubleshooting

### View Application Logs
- Azure Portal → Your App Service → **Monitoring** → **Log stream**

### Common Issues
- **Connection fails**: Verify connection string and SQL Database firewall settings
- **App won't start**: Check logs and ensure .NET 9.0 runtime is configured
- **Database access denied**: Ensure "Allow Azure services" is enabled in SQL Database firewall

## Update Deployment

To update your application:
1. Make code changes
2. Run `.\deploy.ps1` again
3. Upload the new `app.zip` file
4. Your app will restart automatically

## File Structure After Deployment

The `app.zip` contains:
- Compiled .NET application
- All dependencies
- Static files (CSS, JS, images)
- Configuration files

This approach gives you full control over the deployment process while keeping it simple and straightforward.
