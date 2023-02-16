FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity.WebAPI/Identity.WebAPI.csproj", "Identity.WebAPI/"]
COPY ["Services/Identity/Identity.Application/Identity.Application.csproj", "Identity.Application/"]
COPY ["Services/Identity/Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity.Infrastructure/"]
RUN dotnet restore "Identity.WebAPI/Identity.WebAPI.csproj"
COPY ./Services/Identity/ .
WORKDIR "/src/Identity.WebAPI"
RUN dotnet build "Identity.WebAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.WebAPI.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.WebAPI.dll"]
