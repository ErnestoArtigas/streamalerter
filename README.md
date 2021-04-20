# StreamAlerter

## Explication

Projet de Master sur les WebServices qui se décompose en deux parties :
- StreamAlerter.API : Une API Rest qui contacte l'API de Twitch pour récupérer la liste des Streamers enregistrés qui sont en live. Il est codé en C# avec l'utilisation du framework [.NET](https://dotnet.microsoft.com).
- StreamAlerter Bot : Un Bot Discord qui contacte périodiquement l'API Rest pour savoir si les streamers sont en live ou non. S'ils le sont, le bot envoie un message pour annoncer le live. Il est codé en [Node.js](https://nodejs.org/en/) avec l'utilisation du framework [discord.js](https://discord.js.org/#/).

Le projet est fait en pair programming, les conditions sanitaires nous obligeant à utiliser le Live Share de Visual Studio et Visual Studio Code pour travailler. Les commits seront donc très souvent assignés à la même personne pour cette raison.

## Auteurs
- Ernesto ARTIGAS
- Alexandre FOVET