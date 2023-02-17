dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
reportgenerator "-reports:src/*/*.xml" "-targetdir:CoverageReport"


