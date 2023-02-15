FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/AuthService/AuthService.WebAPI/AuthService.WebAPI.csproj", "AuthService.WebAPI/"]
COPY ["Services/AuthService/AuthService.Application/AuthService.Application.csproj", "AuthService.Application/"]
COPY ["Services/AuthService/AuthService.Domain/AuthService.Domain.csproj", "AuthService.Domain/"]
COPY ["Services/AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "AuthService.Infrastructure/"]
RUN dotnet restore "AuthService.WebAPI/AuthService.WebAPI.csproj"
COPY ./Services/AuthService/ .
WORKDIR "/src/AuthService.WebAPI"
RUN dotnet build "AuthService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthService.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.WebAPI.dll"]
