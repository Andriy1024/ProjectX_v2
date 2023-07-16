```
terraform init -upgrade

terraform plan -out main.tfplan

terraform apply main.tfplan
```

Run az aks list to display the name of the new Kubernetes cluster.
```
az aks list \
  --resource-group $resource_group_name \
  --query "[].{\"K8s cluster name\":name}" \
  --output table
```

Set an environment variable so that kubectl picks up the correct config.
```
export KUBECONFIG=./kubeconfig

kubectl get nodes

kubectl get pods -n kube-system | grep addon

kubectl get pods --all-namespaces

kubectl get service --all-namespaces

kubectl apply -f hello-world-ingress.yaml

kubectl get ingress
```

Delete AKS resources
```
terraform plan -destroy -out main.destroy.tfplan

terraform apply main.destroy.tfplan
```

Delete service principal
```
sp=$(terraform output -raw sp)

az ad sp delete --id $sp
```