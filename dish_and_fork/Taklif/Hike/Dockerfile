#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

WORKDIR /src
COPY ["Hike.csproj", "./" ]
RUN dotnet restore "Hike.csproj"
COPY . .
WORKDIR "/src/clientapp/"
RUN npm i --legacy-peer-deps
RUN npm run lint
ARG REACT_APP_API_BASE_URL
ENV REACT_APP_API_BASE_URL=$REACT_APP_API_BASE_URL
ARG REACT_APP_SOCKET_HUBS
ENV REACT_APP_SOCKET_HUBS=$REACT_APP_SOCKET_HUBS
ARG REACT_APP_API_AUTH_CONFIG_URL
ENV REACT_APP_API_AUTH_CONFIG_URL=$REACT_APP_API_AUTH_CONFIG_URL
ARG REACT_APP_GA_ID
ENV REACT_APP_GA_ID=$REACT_APP_GA_ID
RUN echo $REACT_APP_API_BASE_URL
RUN echo $REACT_APP_SOCKET_HUBS
RUN echo $REACT_APP_API_AUTH_CONFIG_URL
RUN echo $REACT_APP_GA_ID
RUN rm .env
RUN echo "BROWSER=none" > .env
RUN echo "REACT_APP_API_AUTH_CLIENT_ID=Hike" >> .env
RUN echo "REACT_APP_TELEGRAM_URL=https://t.me/dishfork" >> .env
RUN echo "REACT_APP_API_BASE_URL=$REACT_APP_API_BASE_URL" >> .env
RUN echo "REACT_APP_SOCKET_HUBS=$REACT_APP_SOCKET_HUBS" >> .env
RUN echo "REACT_APP_API_AUTH_CONFIG_URL=$REACT_APP_API_AUTH_CONFIG_URL" >> .env
RUN echo "REACT_APP_GA_ID=$REACT_APP_GA_ID" >> .env
WORKDIR "/src/"
RUN dotnet build "Hike.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hike.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hike.dll"]
