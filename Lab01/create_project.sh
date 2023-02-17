dotnet new sln
mkdir src
dotnet new classlib -o src/Lab01.Domain
dotnet new xunit -o src/Lab01.Domain.Tests
dotnet sln add src/Lab01.Domain/Lab01.Domain.csproj
dotnet sln add src/Lab01.Domain.Tests/Lab01.Domain.Tests.csproj
dotnet add src/Lab01.Domain.Tests/Lab01.Domain.Tests.csproj reference src/Lab01.Domain/Lab01.Domain.csproj 
