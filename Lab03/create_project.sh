dotnet new sln
mkdir src
dotnet new classlib -o src/Lab03.Domain
dotnet new xunit -o src/Lab03.Domain.Tests
dotnet add src/Tests/Lab03.Tests.csproj reference src/Lab03.Domain.csproj
dotnet sln add src/Lab03.Domain/Lab03.Domain.csproj
dotnet add src/Lab03.Domain.Tests/Lab03.Domain.Tests.csproj reference src/Lab03.Domain/Lab03.Domain.csproj 
