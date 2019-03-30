FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DataManager/DataManager.csproj", "DataManager/"]
RUN dotnet restore "DataManager/DataManager.csproj"
COPY . .
WORKDIR "/src/DataManager"
RUN dotnet build "DataManager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DataManager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DataManager.dll"]