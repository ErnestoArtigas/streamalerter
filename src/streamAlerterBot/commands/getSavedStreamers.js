const { callGetApi } = require("../callFunctions.js");

module.exports = {
	name: "getSavedStreamers",
	description: "Get all the saved streamers in the database.",
	execute(message) {
		message.channel.send("The saved streamers are :");
		callGetApi("https://localhost:44377/StreamAlerter").then((savedStreamers) => {
			savedStreamers.forEach(function(element) {
				const textMessage = element.name + " - https://www.twitch.tv/" + element.name;
				message.channel.send(textMessage);
			});
		}).catch((error) => console.log("error", error));
	}
};