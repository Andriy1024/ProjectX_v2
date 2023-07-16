resource "random_string" "my_numbers" {
  length  = 5
  special = false
  upper   = false
}

resource "azurerm_storage_account" "storage_account" {
  name                          = "projectx${random_string.my_numbers.result}"
  resource_group_name           = azurerm_resource_group.rg_project_x.name
  location                      = azurerm_resource_group.rg_project_x.location
  account_tier                  = "Standard"
  account_replication_type      = "LRS"
  #public_network_access_enabled = false

  network_rules {
    default_action             = "Deny"
    ip_rules                   = [var.my_local_ip]
    #virtual_network_subnet_ids = [azurerm_subnet.vnet_01_subnet_01.id]
  }

#   identity {
#     type = "UserAssigned"
#     identity_ids = [
#       azurerm_user_assigned_identity.user_identity.id
#     ]
#   }

  tags = {
    Terraform = true
    Project   = "project-x"
    Owner     = data.azuread_user.current_user.user_principal_name
  }
}

resource "azurerm_storage_container" "projext_x_container" {
  name                 = "project-x-files"
  storage_account_name = azurerm_storage_account.storage_account.name
  container_access_type = "private"

  depends_on = [
    azurerm_storage_account.storage_account,
  ]
}