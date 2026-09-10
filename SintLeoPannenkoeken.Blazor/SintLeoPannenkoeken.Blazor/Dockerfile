FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["SintLeoPannenkoeken.sln", "."]
COPY ["SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor.csproj", "SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor/"]
COPY ["SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor.Client/SintLeoPannenkoeken.Blazor.Client.csproj", "SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor.Client/"]
COPY ["SintLeoPannenkoeken.ServiceDefaults/SintLeoPannenkoeken.ServiceDefaults.csproj", "SintLeoPannenkoeken.ServiceDefaults/"]
RUN dotnet restore "SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor.csproj"

COPY . .
WORKDIR "/src/SintLeoPannenkoeken.Blazor/SintLeoPannenkoeken.Blazor"
RUN dotnet publish "SintLeoPannenkoeken.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SintLeoPannenkoeken.Blazor.dll"]
