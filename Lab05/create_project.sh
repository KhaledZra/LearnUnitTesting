dotnet new sln
mkdir src
dotnet new classlib -o src/Lab05.Domain
dotnet new xunit -o src/Lab05.Domain.Tests
dotnet add src/Tests/Lab05.Tests.csproj reference src/Lab05.Domain.csproj
dotnet sln add src/Lab05.Domain/Lab05.Domain.csproj
dotnet add src/Lab05.Domain.Tests/Lab05.Domain.Tests.csproj reference src/Lab05.Domain/Lab05.Domain.csproj 
