const https = require('https');
var unirest = require('unirest');
const Discord = require('discord.js');
const { prefix, token } = require('./config.json');

const client = new Discord.Client();

client.once('ready', () => {
	console.log('Ready!');

	setInterval(function() {
		callGetApi("https://localhost:44377/StreamAlerter/getLiveStreamers").then((liveStreamers) => {
			console.log("result : ", liveStreamers)
			if (liveStreamers.length > 0) {
				liveStreamers.forEach(element => {
					client.channels.cache.get("834030808195923982").send(specificMessage(element.name));
				});
			}
		}).catch((error) => console.log("error", error));
	}, 3000);

	/*
	// Once connected, verify each X time if there are any stramers living, if so, send a message
	setInterval(function() {
		callGetApi("https://localhost:44377/StreamAlerter/getLiveStreamers").then((liveStreamers) => {
			console.log("result : ", liveStreamers)
			if (liveStreamers.length > 0) {
				liveCachedStreamer = liveStreamers;
				client.channels.cache.get("834030808195923982").send("!getLiveStreamers").then((msg) => {
					liveCachedStreamer = [];
					msg.delete({timeout: 10000});
				});
			}
		}).catch((error) => console.log("error", error));
	}, 3000);
	*/
});

client.login(token);


function listLiveStreamers(liveStreamers) {
	liveStreamers.forEach(element => {
		message.channel.send(specificMessage(element.name));
	});
}

client.on('message', message => {

	// If not good commmand or 
	if (!message.content.startsWith(prefix)) return;

	// Store args to reuse later for verification
	const args = message.content.slice(prefix.length).trim().split(" ");
	const command = args.shift();

	// Get all streamers living 
		/*
	if (command === "getLiveStreamers") {
		if (liveCachedStreamer.length > 0) {
			console.log("Cached : ", liveCachedStreamer)
			liveCachedStreamer.forEach(element => {
				message.channel.send(specificMessage(element.name));
			});
			liveCachedStreamer = [];
		}
		else {
			callGetApi("https://localhost:44377/StreamAlerter/getLiveStreamers").then((liveStreamers) => {
				liveStreamers.forEach(element => {
					message.channel.send(specificMessage(element.name));
				});
			}).catch((error) => console.log("error", error));
		}
	}
	*/

	// Get all streamers in database
	if (command === "getSavedStreamers") {
		message.channel.send('Les streamers sauvegardés sont :');
		callGetApi("https://localhost:44377/StreamAlerter").then((savedStreamers) => {
			savedStreamers.forEach(function(element) {
				let textMessage = element.name + " - https://www.twitch.tv/" + element.name;
				message.channel.send(textMessage);
			});
		}).catch((error) => console.log("error", error));
	}

	// Add a streamer in database and if bot message, we will ignore
	else if (command === "addStreamer" && !message.author.bot) {
		// Verifying the args
		if (!args.length || args.length > 1) {
			message.channel.send("Wrong syntax. You need to write : ");
			message.channel.send("!addStreamer nameOfYouStreamer");
		}
		else {
			callPostApi("https://localhost:44377/StreamAlerter", args[0]).then((insertedStreamer) => {
				let textMessage = insertedStreamer.name + " was inserted.";
				let textMessage2 = "His channel is - https://www.twitch.tv/" + insertedStreamer.name;
				message.channel.send(textMessage);
				message.channel.send(textMessage2);
			}).catch((error) => console.log("error", error));
		}
	}

	// Add a streamer in database and if bot message, we will ignore
	else if (command === "deleteStreamer" && !message.author.bot) {
		// Verifying the args
		if (!args.length || args.length > 1) {
			message.channel.send("Wrong syntax. You need to write : ");
			message.channel.send("!deleteStreamer nameOfYouStreamer");
		}
		else {
			callDeleteApi("https://localhost:44377/StreamAlerter", args[0]).then((result) => {
				console.log("Result => ", result)
				if (!result)
					message.channel.send("The streamer wasn't found in the database.");
				else {
					let textMessage = args[0] + " was deleted.";
					message.channel.send(textMessage);
				}
			}).catch((error) => console.log("error", error));
		}
	}

}); // End of client.on message 

// Specific message to announce a stramer living
function specificMessage(streamerName) {
	return streamerName + " is Streaming. Go watch his live on https://www.twitch.tv/" + streamerName
}

/// --- Calling API functions --- 

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

function callPostApi(link, streamerName) {
	return new Promise((resolve, reject) => {
		var req = unirest('POST', link)
		.strictSSL(false)
		.headers({
			'Content-Type': 'application/json'
		})
		.send(JSON.stringify({
			"name": streamerName,
			"inLive": false
		}))
		.end(function (res) { 
			if (res.error)
				return reject(res.error);
			else
				return resolve(JSON.parse(res.raw_body));
		});
	})
}

function callDeleteApi(link, streamerName) {
	return new Promise((resolve, reject) => {
		var req = unirest('Delete', link+"/"+streamerName)
		.strictSSL(false)
		.end(function (res) { 
			if (res.error)
				return reject(res.error);
			else
				return resolve((Boolean)(res.raw_body));
		});
	})
}