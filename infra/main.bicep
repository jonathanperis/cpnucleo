@description('Azure region for all resources.')
param location string = 'brazilsouth'

@description('Name of the resource group (used for tagging).')
param projectName string = 'github-jonathanperis'

@description('Name of the App Service Plan (shared across all projects).')
param appServicePlanName string = 'github-jonathanperis'

@description('App Service Plan SKU.')
@allowed(['F1', 'B1', 'B2', 'B3', 'S1', 'S2', 'S3', 'P1v3', 'P2v3', 'P3v3'])
param appServicePlanSku string = 'F1'

@description('Name of the Log Analytics Workspace for cpnucleo.')
param logAnalyticsWorkspaceName string = 'cpnucleo-workspace'

// ── App Service Plan (shared with blazor-mudblazor, managed here) ───────────
module appServicePlan 'modules/appServicePlan.bicep' = {
  name: 'deploy-app-service-plan'
  params: {
    location: location
    appServicePlanName: appServicePlanName
    sku: appServicePlanSku
    projectName: projectName
  }
}

// ── Log Analytics Workspace ─────────────────────────────────────────────────
module logAnalytics 'modules/logAnalytics.bicep' = {
  name: 'deploy-log-analytics'
  params: {
    location: location
    workspaceName: logAnalyticsWorkspaceName
    projectName: projectName
  }
}

// ── Application Insights (one per service) ──────────────────────────────────
module appInsightsWebApi 'modules/appInsights.bicep' = {
  name: 'deploy-app-insights-webapi'
  params: {
    location: location
    appInsightsName: 'cpnucleo-api-dotnet'
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
    projectName: projectName
  }
}

module appInsightsGrpcServer 'modules/appInsights.bicep' = {
  name: 'deploy-app-insights-grpcserver'
  params: {
    location: location
    appInsightsName: 'cpnucleo-grpc-server'
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
    projectName: projectName
  }
}

module appInsightsIdentityApi 'modules/appInsights.bicep' = {
  name: 'deploy-app-insights-identityapi'
  params: {
    location: location
    appInsightsName: 'cpnucleo-identity-api'
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
    projectName: projectName
  }
}

module appInsightsWebClient 'modules/appInsights.bicep' = {
  name: 'deploy-app-insights-webclient'
  params: {
    location: location
    appInsightsName: 'cpnucleo-webclient-dotnet'
    logAnalyticsWorkspaceId: logAnalytics.outputs.workspaceId
    projectName: projectName
  }
}

// ── Web Apps (one per service) ───────────────────────────────────────────────
module webApiApp 'modules/webApp.bicep' = {
  name: 'deploy-webapp-webapi'
  params: {
    location: location
    webAppName: 'cpnucleo-api-dotnet'
    appServicePlanId: appServicePlan.outputs.planId
    containerImage: 'ghcr.io/jonathanperis/cpnucleo-web-api:latest'
    appInsightsConnectionString: appInsightsWebApi.outputs.connectionString
    appInsightsInstrumentationKey: appInsightsWebApi.outputs.instrumentationKey
    projectName: projectName
  }
}

module grpcServerApp 'modules/webApp.bicep' = {
  name: 'deploy-webapp-grpcserver'
  params: {
    location: location
    webAppName: 'cpnucleo-grpc-server'
    appServicePlanId: appServicePlan.outputs.planId
    containerImage: 'ghcr.io/jonathanperis/cpnucleo-grpc-server:latest'
    appInsightsConnectionString: appInsightsGrpcServer.outputs.connectionString
    appInsightsInstrumentationKey: appInsightsGrpcServer.outputs.instrumentationKey
    projectName: projectName
  }
}

module identityApiApp 'modules/webApp.bicep' = {
  name: 'deploy-webapp-identityapi'
  params: {
    location: location
    webAppName: 'cpnucleo-identity-api'
    appServicePlanId: appServicePlan.outputs.planId
    containerImage: 'ghcr.io/jonathanperis/cpnucleo-identity-api:latest'
    appInsightsConnectionString: appInsightsIdentityApi.outputs.connectionString
    appInsightsInstrumentationKey: appInsightsIdentityApi.outputs.instrumentationKey
    projectName: projectName
  }
}

module webClientApp 'modules/webApp.bicep' = {
  name: 'deploy-webapp-webclient'
  params: {
    location: location
    webAppName: 'cpnucleo-webclient-dotnet'
    appServicePlanId: appServicePlan.outputs.planId
    containerImage: 'ghcr.io/jonathanperis/cpnucleo-web-client:latest'
    appInsightsConnectionString: appInsightsWebClient.outputs.connectionString
    appInsightsInstrumentationKey: appInsightsWebClient.outputs.instrumentationKey
    projectName: projectName
  }
}

// ── Outputs ──────────────────────────────────────────────────────────────────
output webApiUrl string = webApiApp.outputs.defaultHostname
output grpcServerUrl string = grpcServerApp.outputs.defaultHostname
output identityApiUrl string = identityApiApp.outputs.defaultHostname
output webClientUrl string = webClientApp.outputs.defaultHostname
