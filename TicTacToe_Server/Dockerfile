FROM mcr.microsoft.com/dotnet/sdk:3.1

COPY bin/Release/netcoreapp3.1/publish/ APP/
WORKDIR /APP

EXPOSE 8005

ENTRYPOINT ["dotnet", "TicTacToe_Server.dll"]