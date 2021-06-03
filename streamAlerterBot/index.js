const https = require('https');
var unirest = require('unirest');
const Discord = require('discord.js');
const { prefix, token } = require('./config.json');

const client = new Discord.Client();

client.once('ready', () => {
	console.log('Ready!');
});

client.login(token);

client.on('message', message => {
	if (message.content === `${prefix}getLiveStreamers`) {
		message.channel.send('Les streamers en live sont :');
		callGetApi("https://localhost:44377/StreamAlerter/getLiveStreamers").then((liveStreamers) => {
			liveStreamers.forEach(element => {
				message.channel.send(specificMessage(element.name));
			});
		}).catch((error) => console.log("error", error));
	}

	else if (message.content === `${prefix}getSavedStreamers`) {
		message.channel.send('Les streamers sauvegardés sont :');
		callGetApi("https://localhost:44377/StreamAlerter/getSavedStreamers").then((savedStreamers) => {
			savedStreamers.forEach(function(element) {
				let textMessage = element.name + " - https://www.twitch.tv/" + element.name;
				message.channel.send(textMessage);
			});
		}).catch((error) => console.log("error", error));
	}

});

function specificMessage(streamerName) {
	return streamerName + " is Streaming. Go watch his live on https://www.twitch.tv/" + streamerName
}

function callGetApi(link) {
	return new Promise((resolve, reject) => {
		var req = unirest('GET', link)
		.strictSSL(false)
		.end(function (res) { 
			if (res.error)
				return reject(res.error);
			else
				return resolve(JSON.parse(res.raw_body));
		});
	})
}