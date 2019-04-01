FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Frontend/ControlPanel.csproj", "Frontend/"]
RUN dotnet restore "Frontend/ControlPanel.csproj"
COPY . .
WORKDIR "/src/Frontend"
RUN dotnet build "ControlPanel.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ControlPanel.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ControlPanel.dll"]