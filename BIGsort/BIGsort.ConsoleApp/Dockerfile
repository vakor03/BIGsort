FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BIGsort.ConsoleApp/BIGsort.ConsoleApp.csproj", "BIGsort.ConsoleApp/"]
RUN dotnet restore "BIGsort.ConsoleApp/BIGsort.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/BIGsort.ConsoleApp"
RUN dotnet build "BIGsort.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BIGsort.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BIGsort.ConsoleApp.dll"]
