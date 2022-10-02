const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast"
      //"/api/metadata/GetServers",
    ],
    target: "https://localhost:7115",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
