const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target: "https://datacommandcenterapi20221014132559.azurewebsites.net/",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
