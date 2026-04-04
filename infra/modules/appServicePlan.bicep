param location string
param appServicePlanName string
param sku string
param projectName string

var skuTierMap = {
  F1: 'Free'
  B1: 'Basic'
  B2: 'Basic'
  B3: 'Basic'
  S1: 'Standard'
  S2: 'Standard'
  S3: 'Standard'
  P1v3: 'PremiumV3'
  P2v3: 'PremiumV3'
  P3v3: 'PremiumV3'
}

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  tags: {
    project: projectName
  }
  sku: {
    name: sku
    tier: skuTierMap[sku]
  }
  properties: {
    reserved: true // required for Linux
  }
}

output planId string = appServicePlan.id
output planName string = appServicePlan.name
