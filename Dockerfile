FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
LABEL maintainer="Ife Ayelabola"
WORKDIR /src
COPY PilotScheduler.Service/*.csproj /src/PilotScheduler.Service/
COPY PilotScheduler.Repository/*.csproj /src/PilotScheduler.Repository/
RUN dotnet restore /src/PilotScheduler.Service/
COPY PilotScheduler.Service/. /src/PilotScheduler.Service/
COPY PilotScheduler.Repository/. /src/PilotScheduler.Repository/

RUN dotnet publish PilotScheduler.Service/PilotScheduler.Service.csproj -c Release -o out
RUN mkdir ./out/data

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /src
COPY --from=build /src/out .
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "PilotScheduler.Service.dll"]

