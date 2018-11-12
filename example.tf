provider azurerm {

}

resource "azurerm_resource_group" "rg" {
  name = "example-rg"
  location = "eastus2"
}

resource "azurerm_virtual_network" "vnet" {
  name = "example-vn"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  location = "${azurerm_resource_group.rg.location}"
  address_space = ["10.0.0.0/20"]
}

resource "azurerm_subnet" "gw-sn" {
  name = "GatewaySubnet"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  virtual_network_name = "${azurerm_virtual_network.vnet.name}"
  address_prefix = "10.0.0.0/27"
}

resource "azurerm_subnet" "app-sn" {
  name = "AppSubnet"
  resource_group_name = "${azurerm_resource_group.rg.name}"
  virtual_network_name = "${azurerm_virtual_network.vnet.name}"
  address_prefix = "10.0.0.32/27"
}