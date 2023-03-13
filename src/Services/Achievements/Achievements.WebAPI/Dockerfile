FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Users.WebAPI/Users.WebAPI.csproj", "Users.WebAPI/"]
RUN dotnet restore "Users.WebAPI/Users.WebAPI.csproj"
COPY . .
WORKDIR "/src/Users.WebAPI"
RUN dotnet build "Users.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.WebAPI.dll"]
