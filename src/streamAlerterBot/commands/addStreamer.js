const { callPostApi } = require("../callFunctions.js");

module.exports = {
	name: "addStreamer",
	description: "Add a streamer to the database.",
	args: true,
	argsLength: 1,
	usage: "<nameOfStreamer>",
	execute(message, args) {
		if (!args.length || args.length > 1) {
			message.channel.send("Wrong syntax. You need to write : ");
			message.channel.send("!addStreamer nameOfYouStreamer");
		}
		else {
			callPostApi("https://localhost:44377/StreamAlerter", args[0]).then((insertedStreamer) => {
				const textMessage = insertedStreamer.name + " was inserted.";
				const textMessage2 = "His channel is - https://www.twitch.tv/" + insertedStreamer.name;
				message.channel.send(textMessage);
				message.channel.send(textMessage2);
			}).catch((error) => console.log("error", error));
		}
	}
};