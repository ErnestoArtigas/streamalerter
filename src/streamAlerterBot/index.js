const fs = require("fs");
const Discord = require("discord.js");
const { prefix, token } = require("./config.json");
const { callGetApi } = require("./callFunctions.js");
var { channelID } = require("./config.json");

const client = new Discord.Client();
client.commands = new Discord.Collection();

const commandFiles = fs.readdirSync("./commands").filter(file => file.endsWith(".js"));

commandFiles.forEach(element => {
	const command = require(`./commands/${element}`);
	client.commands.set(command.name, command);
});

client.once("ready", () => {
	console.log("Ready!");

	console.log(channelID);

	/*
	* Each 3 seconds we do a get request to the API for searching the current streamers.
	* If the channel was prompted we can search the living streamers and print them.
	* To avoid any problems of saving the wrong channel of a distinct server we verify if it's undefined.
	*/
	setInterval(function() {
		if (client.channels.cache.get(channelID) != undefined) {
			callGetApi("https://localhost:44377/StreamAlerter/getLiveStreamers").then((liveStreamers) => {
				console.log("result : ", liveStreamers);
				if (liveStreamers.length > 0) {
					liveStreamers.forEach(element => {
						client.channels.cache.get(channelID).send(specificMessage(element.name));
					});
				}
			}).catch((error) => console.log("error", error));
		}
	}, 3000);
});

client.login(token);

client.on("message", message => {

	// If the command doesn't begin with the prefix specified in config.json and isn't made by the bot.
	if (!message.content.startsWith(prefix) && !message.author.bot) return;

	// Storing of the arguments and the command for more readibility.
	const args = message.content.slice(prefix.length).trim().split(/ +/);
	const commandName = args.shift();

	if (!client.commands.has(commandName)) return;

	const command = client.commands.get(commandName);

	if (command.args && args.length != command.argsLength) {
		let reply = "You didn't provide any or too many arguments !";

		if (command.usage)
			reply += `\nThe proper usage would be : \`${prefix}${commandName} ${command.usage}\``;

		return message.channel.send(reply);
	}

	try {
		command.execute(message, args);
	}
	catch (error) {
		console.error(error);
		message.reply("There was an error trying to execute that command!");
	}
});

function specificMessage(streamerName) {
	return streamerName + " is Streaming. Go watch his live on https://www.twitch.tv/" + streamerName;
}