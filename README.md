# StreamAlerter

## Explication

Projet de Master sur les WebServices qui se décompose en deux parties :
- Une API Rest qui contacte l'API de Twitch pour récupérer la liste des Streamers enregistrés qui sont en live.
- Un Bot Discord qui contacte périodiquement l'API Rest pour savoir si les streamers sont en live ou non. S'ils le sont, le bot envoie un message pour annoncer le live.