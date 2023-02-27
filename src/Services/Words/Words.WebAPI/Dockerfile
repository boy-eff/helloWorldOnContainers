FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Words/Words.WebAPI/Words.WebAPI.csproj", "Services/Words/Words.WebAPI/"]
COPY ["Services/Words/Words.BusinessAccess/Words.BusinessAccess.csproj", "Services/Words/Words.BusinessAccess/"]
COPY ["Services/Words/Words.DataAccess/Words.DataAccess.csproj", "Services/Words/Words.DataAccess/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Services/Words/Words.WebAPI/Words.WebAPI.csproj"
COPY ./Services/Words/ ./Services/Words/
COPY ./Shared/ ./Shared
WORKDIR "/src/Services/Words/Words.WebAPI"
RUN dotnet build "Words.WebAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Words.WebAPI.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Words.WebAPI.dll"]
