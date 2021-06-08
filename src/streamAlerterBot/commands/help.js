const { prefix } = require("../config.json");

module.exports = {
	name: "help",
	description: "List all commands or info about a specific command.",
	usage: "<commandName>",
	execute(message, args) {
		const data = [];
		const { commands } = message.client;

		if (!args.length) {
			data.push("Here's a list of all the commands :");
			data.push(commands.map(command => command.name).join("\n"));
			data.push(`\nYou can send \`${prefix}help <commandName>\` to get info on a specific command!`);

			return message.channel.send(data, { split: true });
		}

		const name = args[0];
		const command = commands.get(name);

		if (!command)
			return message.channel.send("That is not a valid command!");

		data.push(`**Name:** ${command.name}`);

		if (command.description)
			data.push(`**Description:** ${command.description}`);

		if (command.usage)
			data.push(`**Usage:** ${prefix}${command.name} ${command.usage}`);

		message.channel.send(data, { split: true });
	}
};