Write-Output "Packaging services...";

Set-Location ./AirMonitoring.DataProviding
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-data-providing.zip
Write-Output ">>> Finished packaging AirMonitoring.DataProviding";
Set-Location ..

Set-Location ./AirMonitoring.DataIngestion
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-data-ingestion.zip
Write-Output ">>> Finished packaging AirMonitoring.DataIngestion";
Set-Location ..

Set-Location ./AirMonitoring.DataValidation
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-data-validation.zip
Write-Output ">>> Finished packaging AirMonitoring.DataValidation";
Set-Location ..

Set-Location ./AirMonitoring.DataAnalysis
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-data-analysis.zip
Write-Output ">>> Finished packaging AirMonitoring.DataAnalysis";
Set-Location ..

Set-Location ./AirMonitoring.Alerting
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-alerting.zip
Write-Output ">>> Finished packaging AirMonitoring.Alerting";
Set-Location ..

Set-Location ./AirMonitoring.CommandProcessing
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-command-processing.zip
Write-Output ">>> Finished packaging AirMonitoring.CommandProcessing";
Set-Location ..

Set-Location ./AirMonitoring.MonthlyReporting
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-monthly-reporting.zip
Write-Output ">>> Finished packaging AirMonitoring.MonthlyReporting";
Set-Location ..

Set-Location ./AirMonitoring.YearlyReporting
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-yearly-reporting.zip
Write-Output ">>> Finished packaging AirMonitoring.YearlyReporting";
Set-Location ..

Set-Location ./AirMonitoring.LatestMeasurements
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-latest-measurements.zip
Write-Output ">>> Finished packaging AirMonitoring.LatestMeasurements";
Set-Location ..

Set-Location ./AirMonitoring.Aggregation
dotnet restore
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package-aggregation.zip
Write-Output ">>> Finished packaging AirMonitoring.Aggregation";
Set-Location ..

Write-Output ">>> >>> >>> Finished all services.";