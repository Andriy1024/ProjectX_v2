resource "random_pet" "azurerm_kubernetes_cluster_dns_prefix" {
  prefix = "dns"
}

resource "azurerm_kubernetes_cluster" "k8s" {
  name                = "project-x-cluster"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  dns_prefix          = random_pet.azurerm_kubernetes_cluster_dns_prefix.id

#   addon_profile {
#     http_application_routing {
#         enabled = true
#     }
#   }

   http_application_routing_enabled = true
#   ingress_application_gateway {
#     subnet_id = azurerm_subnet.appgwsubnet.id
#     subnet_cidr = "10.0.0.0/24"
#     gateway_name = "agic-appgw"
#   }

  identity {
    type = "SystemAssigned"
  }

  default_node_pool {
    name       = "agentpool"
    node_count = "2"
    vm_size    = "standard_d2_v2"
    enable_auto_scaling = false
  }

  linux_profile {
    admin_username = "ubuntu"

    ssh_key {
      key_data = jsondecode(azapi_resource_action.ssh_public_key_gen.output).publicKey
    }
  }

  network_profile {
    network_plugin    = "kubenet"
    load_balancer_sku = "standard"
  }
}