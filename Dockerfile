# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore first for better cache usage
COPY ["Assignment.Task.csproj", "./"]
RUN dotnet restore "Assignment.Task.csproj"

# Copy the rest of the sources and publish
COPY . .
RUN dotnet publish "Assignment.Task.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS="http://+:8080"

# Copy published output
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Assignment.Task.dll"]

