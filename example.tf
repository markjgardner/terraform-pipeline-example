variable "tenantId" {
  description = "The AzureAD tenant"
}

variable "subscriptionId" {
  description = "The azure subscription"
}

variable "clientId" {
  description = "The application ID of the service principal"
}

variable "clientSecret" {
  description = "The secret key for the service principal"
}

variable "vnetAddressSpace" {
  description = "The address space allocated to the vnet"
  default = "10.0.0.0/20"
}


terraform {
  backend "azurerm" {
    container_name = "terraform"
    key            = "example"
  }
}

provider "azurerm" {
  version         = "~> 1.32"
  tenant_id       = var.tenantId
  subscription_id = var.subscriptionId
  client_id       = var.clientId
  client_secret   = var.clientSecret
}

resource "azurerm_resource_group" "rg" {
  name     = "tf-example-${terraform.workspace}-rg"
  location = "eastus2"
}

resource "azurerm_virtual_network" "vnet" {
  name                = "tf-example-${terraform.workspace}-vn"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  address_space       = [var.vnetAddressSpace]
}

resource "azurerm_subnet" "gw-sn" {
  name                 = "GatewaySubnet"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefix       = cidrsubnet(var.vnetAddressSpace, 7, 0)
}

resource "azurerm_subnet" "app-sn" {
  name                 = "AppSubnet"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefix       = cidrsubnet(var.vnetAddressSpace, 7, 1)
}

resource "azurerm_subnet" "app2-sn" {
  name                 = "App2Subnet"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefix       = cidrsubnet(var.vnetAddressSpace, 7, 2)
}
