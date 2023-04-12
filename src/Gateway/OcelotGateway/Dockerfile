FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Gateway/OcelotGateway/OcelotGateway.csproj", "Gateway/OcelotGateway/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Gateway/OcelotGateway/OcelotGateway.csproj"
COPY ./Gateway/OcelotGateway/ ./Gateway/OcelotGateway
COPY ./Shared/ ./Shared/
WORKDIR "/src/Gateway/OcelotGateway"
RUN dotnet build "OcelotGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotGateway.dll"]
