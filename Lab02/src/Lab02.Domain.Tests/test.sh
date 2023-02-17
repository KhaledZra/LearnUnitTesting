dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
reportgenerator "-reports:./coverage.opencover.xml" "-targetdir:CoverageReport"


