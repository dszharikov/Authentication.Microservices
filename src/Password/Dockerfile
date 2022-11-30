FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Password/Password.csproj", "Password/"]
RUN dotnet restore "src/Password/Password.csproj"
COPY . .
WORKDIR "/src/Password"
RUN dotnet build "Password.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Password.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Password.dll"]
