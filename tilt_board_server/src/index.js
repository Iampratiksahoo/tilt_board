const express = require('express');
const bodyParser = require('body-parser');
const http = require('http');

const config = require('./config/default');
const logger = require('./util/logger');

const { initWsServer } = require('./ws/wsServer');

const app = express();

// middleware 
app.use(bodyParser.json());

// health check 
app.get("/", (req, res) => res.send("Tilt Board game server (Headless) is now running..."));

// Create HTTP server
const server = http.createServer(app);

// Initialize WebSocket server
initWsServer(server);

// Start HTTP server
server.listen(config.HTTP_PORT, () => {
  logger.info(`HTTP + Websocket server listening at http://localhost:${config.HTTP_PORT}`);
});