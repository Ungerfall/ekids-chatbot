$rg = "rg-ekids-chatbot"
$account = "cosmos-ungerfall-ekids-chatbot"
$db = "ekids-chatbot"
$homeworksContainerName = "homeworks"
$location = "westeurope"

$loginStatus = az login
# if the status is null or empty, the login has failed
if ([string]::IsNullOrEmpty($loginStatus)) {
    echo "Failed to login. Exiting the script."
    exit
}

$resourceGroup = az group exists --name $rg
if ($resourceGroup -eq "false") {
    echo "Creating resource group..."
    az group create --name $rg --location $location
} else {
    echo "Resource group already exists."
}

$cosmosDbAccount = az cosmosdb check-name-exists --name $account
if ($cosmosDbAccount -eq "false") {
    echo "Creating Cosmos DB account..."
    az cosmosdb create --name $account --resource-group $rg --locations regionName=$location failoverPriority=0 isZoneRedundant=False
} else {
    echo "Cosmos DB account already exists."
}

echo "Creating/Ensuring the database exists in the Cosmos DB account..."
az cosmosdb sql database create --account-name $account --resource-group $rg --name $db

$homeworksContainer = az cosmosdb sql container exists --account-name $account `
                                 --database-name $db `
                                 --name $homeworksContainerName `
                                 --resource-group $rg
if ($homeworksContainer -eq "false") {
  echo "Creating the empty $homeworksContainerName container..."
    az cosmosdb sql container create --account-name $account `
    --database-name $db `
    --name $homeworksContainerName `
    --partition-key-path "/courseId" `
    --resource-group $rg `
    --query "id" `
} else {
    echo "Cosmos DB homeworks container already exists."
}
