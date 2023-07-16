terraform {
    required_providers {
        azurerm = {
          source  = "hashicorp/azurerm"
          version = "=3.60.0"
        }
    }

  # backend "azurerm" {
  #   resource_group_name  = "tfstateRG01"
  #   storage_account_name = ""
  #   container_name       = "azure-tf-state"
  #   key                  = "terraform.tfstate"
  # }
}

provider "azurerm" {
  # skip_provider_registration = true

  features {}
}

resource "azurerm_resource_group" "rg_project_x" {
  name     = "rg-project-x"
  location = var.az_region

  tags = {
    Terraform = true
    Project   = "project-x"
    Owner     = data.azuread_user.current_user.user_principal_name
  }
}