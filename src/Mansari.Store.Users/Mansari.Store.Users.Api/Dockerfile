FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore "src/Mansari.Store.Users/Mansari.Store.Users.Api/Mansari.Store.Users.Api.csproj"
RUN dotnet publish "src/Mansari.Store.Users/Mansari.Store.Users.Api/Mansari.Store.Users.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Mansari.Store.Users.Api.dll"]
