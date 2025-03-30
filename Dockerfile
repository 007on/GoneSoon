FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GoneSoon/GoneSoon/GoneSoonApi.csproj", "GoneSoon.Api/"]
RUN dotnet restore "GoneSoon/GoneSoon/GoneSoonApi.csproj"
COPY . .
WORKDIR "/src/GoneSoon"
RUN dotnet build "GoneSoonApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GoneSoonApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoneSoonApi.dll"]
