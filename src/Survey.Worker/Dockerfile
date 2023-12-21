#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Survey.Worker/Survey.Worker.csproj", "src/Survey.Worker/"]
COPY ["src/Survey.Infrastructure/Survey.Infrastructure.csproj", "src/Survey.Infrastructure/"]
COPY ["src/Survey.Application/Survey.Application.csproj", "src/Survey.Application/"]
COPY ["src/Survey.Domain/Survey.Domain.csproj", "src/Survey.Domain/"]
COPY ["src/Survey.Shared/Survey.Shared.csproj", "src/Survey.Shared/"]
RUN dotnet restore "src/Survey.Worker/Survey.Worker.csproj"
COPY . .
WORKDIR "/src/src/Survey.Worker"
RUN dotnet build "Survey.Worker.csproj" -c Release -o /app/build


RUN dotnet restore "Survey.Worker.csproj"

FROM build AS publish
RUN dotnet publish "Survey.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Survey.Worker.dll"]