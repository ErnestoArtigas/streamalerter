const replaceJSONProperty = require("replace-json-property");

/* global channelID:writable*/
module.exports = {
	name: "setChannel",
	description: "Set the channel for the messages.",
	args: true,
	argsLength: 1,
	usage: "<nameOfChannel>",
	execute(message, args) {
		if (!args.length || args.length > 1) {
			message.channel.send("Wrong syntax. You need to write : ");
			message.channel.send("!selectChannel nameOfTheChannel");
		}
		else {
			channelID = args[0].substring(2, args[0].length - 1);
			replaceJSONProperty.replace("./config.json", "channelID", channelID);
			console.log(channelID);
			message.channel.send("The channel was defined.");
		}
	}
};