variable "vnetAddressSpace" {
  description = "The address space allocated to the vnet"
  default = "10.0.0.0/20"
}

terraform {
  required_version = ">= 0.12"
  backend "azurerm" {
    resource_group_name  = "debug-rg"
    storage_account_name = "terraformexamplestate"
    container_name       = "terraform"
    key                  = "example"
  }
}

provider "azurerm" {
  version  = "~> 2.37"
  features {}
  skip_provider_registration = true
}

resource "azurerm_resource_group" "rg" {
  name     = "tf-example-${terraform.workspace}-rg"
  location = "eastus2"
  tags     = {
    "terraform" = "true"
  }
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
  address_prefixes     = [cidrsubnet(var.vnetAddressSpace, 7, 0)]
}

resource "azurerm_subnet" "app-sn" {
  name                 = "AppSubnet"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = [cidrsubnet(var.vnetAddressSpace, 7, 1)]
}