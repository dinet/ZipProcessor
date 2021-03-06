FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_9.x | bash - && \
    apt-get install -y build-essential nodejs
RUN npm install -g npm

COPY ["ControlPanel/ControlPanel.csproj", "ControlPanel/"]
RUN dotnet restore "ControlPanel/ControlPanel.csproj"
COPY . .
WORKDIR "/src/ControlPanel"
RUN dotnet build "ControlPanel.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ControlPanel.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ControlPanel.dll"]