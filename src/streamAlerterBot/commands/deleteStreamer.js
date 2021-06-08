const { callDeleteApi } = require("../callFunctions.js");

module.exports = {
	name: "deleteStreamer",
	description: "Delete a streamer from the database.",
	args: true,
	argsLength: 1,
	usage: "<nameOfStreamer>",
	execute(message, args) {
		if (!args.length || args.length > 1) {
			message.channel.send("Wrong syntax. You need to write : ");
			message.channel.send("!deleteStreamer nameOfYouStreamer");
		}
		else {
			callDeleteApi("https://localhost:44377/StreamAlerter", args[0]).then((result) => {
				if (!result)
					message.channel.send("The streamer wasn't found in the database.");
				else {
					const textMessage = args[0] + " was deleted.";
					message.channel.send(textMessage);
				}
			}).catch((error) => console.log("error", error));
		}
	}
};