FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["exam-api-project.csproj", "."]
RUN dotnet restore "exam-api-project.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "exam-api-project.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "exam-api-project.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN mkdir -p /app/downloads
RUN mkdir -p /app/configs
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "exam-api-project.dll"]
