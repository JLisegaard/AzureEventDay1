###########################################################################
#
# Use this script to test locally
#
###########################################################################

# Use the 'Throw-WhenError' function to get a quick feedback on what went wrong.
function Throw-WhenError {
  param (
    [string]
    $msg
  )

  if ($LastExitCode -gt 0) {
    Write-Error $msg
    throw
  }
}

Write-Host "Initialize local deployment" -ForegroundColor Blue

# Login and set the sub to the one we want to use from Azure Portal
# az logout
# az login --allow-no-subscriptions # --use-device-code
# az account set --subscription "d6b4bc51-75a6-4eb4-8cf2-4114beceec76"

# From now on we define and create our ressources

# The resource group is already created by Delegate, 
# but if we wanted to create it from scratch, we would do something like this

#### Example of how to create a resource group via az cli:

# Write-Host "Deploying resource group" -ForegroundColor Yellow

$output = az group create `
--name 'rg_jesperlisegaard' `
--location 'westeurope'
Throw-WhenError -output $output

# Note: use backticks ---> ` <---- to change line

## In your first az cli script, you need to create the following:
# - Storage Account ------------

az storage account create --name 'lisegaardstorage' `
                          --resource-group 'rg_jesperlisegaard' 

# - Function App ------------

az functionapp create --name 'lisegaardfunctionapp' `
                      --resource-group 'rg_jesperlisegaard' `
                      --storage-account 'lisegaardstorage'

# - App Service Plan ------------

az appservice plan create --name 'lisegaardappplan' `
                          --resource-group 'rg_jesperlisegaard'


# Please note that the order matters as the commands are run sequentially...

# You can find the documentation for the az cli commands here:
# https://learn.microsoft.com/en-us/cli/azure/reference-index?view=azure-cli-latest

# Prerequisite
# AZ CLI is installed
# When deploying the script with Powershell you need to enter the following command in order to be able to run the script: 'Set-ExecutionPolicy -Scope Process -ExecutionPolicy  ByPass'
# When the script has run, then go to the portal and check inside your resource group, that the deployment has been successful.