FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Words.WebAPI/Words.WebAPI.csproj", "Words.WebAPI/"]
RUN dotnet restore "Words.WebAPI/Words.WebAPI.csproj"
COPY . .
WORKDIR "/src/Words.WebAPI"
RUN dotnet build "Words.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Words.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Words.WebAPI.dll"]
