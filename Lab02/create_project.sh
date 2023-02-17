dotnet new sln
mkdir src
dotnet new classlib -o src/Lab02.Domain
dotnet new xunit -o src/Lab02.Domain.Tests
dotnet add src/Lab02.Domain.Tests/Lab02.Domain.Tests.csproj reference src/Lab02.Domain/Lab02.Domain.csproj
dotnet sln add src/Lab02.Domain/Lab02.Domain.csproj
dotnet add src/Lab02.Domain.Tests/Lab02.Domain.Tests.csproj reference src/Lab02.Domain/Lab02.Domain.csproj 
