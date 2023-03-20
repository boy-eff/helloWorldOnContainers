FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Achievements/Achievements.WebAPI/Achievements.WebAPI.csproj", "Services/Achievements/Achievements.WebAPI/"]
COPY ["Services/Achievements/Achievements.Application/Achievements.Application.csproj", "Services/Achievements/Achievements.Application/"]
COPY ["Services/Achievements/Achievements.Domain/Achievements.Domain.csproj", "Services/Achievements/Achievements.Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Services/Achievements/Achievements.WebAPI/Achievements.WebAPI.csproj"
COPY ./Services/Achievements/ ./Services/Achievements/
COPY ./Shared/ ./Shared
WORKDIR "/src/Services/Achievements/Achievements.WebAPI"
RUN dotnet build "Achievements.WebAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Achievements.WebAPI.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Achievements.WebAPI.dll"]
