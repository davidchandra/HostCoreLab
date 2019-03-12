FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["HostCoreDocker/HostCoreDocker.csproj", "HostCoreDocker/"]
RUN dotnet restore "HostCoreDocker/HostCoreDocker.csproj"
COPY . .
WORKDIR "/src/HostCoreDocker"
RUN dotnet build "HostCoreDocker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HostCoreDocker.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet"]
CMD ["HostCoreDocker.dll"]
