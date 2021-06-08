const unirest = require("unirest");

// Specific message to announce a stramer living
module.exports.callGetApi = (link) => {
	return new Promise((resolve, reject) => {
		unirest("GET", link)
			.strictSSL(false)
			.end(function(res) {
				if (res.error)
					return reject(res.error);
				else
					return resolve(JSON.parse(res.raw_body));
			});
	});
};

// Promise function to get the result of the post request.
module.exports.callPostApi = (link, streamerName) => {
	return new Promise((resolve, reject) => {
		unirest("POST", link)
			.strictSSL(false)
			.headers({
				"Content-Type": "application/json"
			})
			.send(JSON.stringify({
				"name": streamerName,
				"inLive": false
			}))
			.end(function(res) {
				if (res.error)
					return reject(res.error);
				else
					return resolve(JSON.parse(res.raw_body));
			});
	});
};

// Promise function to get the result of the delete request.
module.exports.callDeleteApi = (link, streamerName) => {
	return new Promise((resolve, reject) => {
		unirest("DELETE", link + "/" + streamerName)
			.strictSSL(false)
			.end(function(res) {
				if (res.error)
					return reject(res.error);
				else
					return resolve((Boolean)(res.raw_body));
			});
	});
};