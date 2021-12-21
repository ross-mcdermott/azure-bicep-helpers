# Overview

Through leverageing a pre-generated JSON based mapping file, generating the required Role Definition Resource IDs becomes transparent and highly supportable compared to the traditional approach of embedding Role Definition Names (GUIDs for built in roles) within bicep files or parameter files.

TL;DR; the latest *wellknownroles.json* can be found on the Releases page.

**With** the mapping file:

```bicep
// Load role definition info. 
var roles = json(loadTextContent('wellknownroles.json'))
var contributorRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions',roles['Contributor'])
var costManagementContributorRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions',roles['Cost Management Contributor'])
```

**Without** the mapping file:

```bicep
var contributorRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions','b24988ac-6180-42a0-ab88-20f7382dd24c')
var costManagementContributorRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions','434105ed-43f6-45c7-a02f-909b2ba83430')
```

As can be seen, it's small but significant simplification in readability.

**Tooling Support**

The Bicep tooling is also smart enough to know when the role name that is being referenced is not available within the map, returning an error as shown below:

![Tooling Support](/.assets/ToolingError.png?raw=true "Tooling Support")

## Full Sample

The following is a full sample based on the Microsoft documented example [here](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/bicep-functions-resource#subscriptionresourceid-example).

```bicep
@description('Principal Id')
param principalId string

@allowed([
  'Owner'
  'Contributor'
  'Reader'
])
@description('Built-in role to assign')
param builtInRoleType string

// load the roles
var roles = json(loadTextContent('wellknownroles.json'))
var mappedRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions',roles[builtInRoleType])

resource myRoleAssignment 'Microsoft.Authorization/roleAssignments@2018-09-01-preview' = {
  name: guid(resourceGroup().id, principalId, mappedRoleDefinitionId)
  properties: {
    roleDefinitionId: mappedRoleDefinitionId
    principalId: principalId
  }
}

```

> *Note:* The 'allowed' referenced above is to keep the example aligned with the Microsoft example. This can be removed if the module should support any role type.

