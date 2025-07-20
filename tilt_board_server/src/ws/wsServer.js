const { Server } = require('ws');
const wsRoutes = require('./wsRoutes');
const logger = require('../util/logger');

let wss = null;

function initWsServer(server) {
  wss = new Server({ server });

  wss.on('connection', (ws) => {
    logger.info('New WS client connected');

    ws.on('message', (data) => {
      wsRoutes.handleMessage(ws, data);
    });

    ws.on('close', () => {
      logger.info('WS client disconnected');
    });
  });

  logger.info('WebSocket server initialized');
}

function getWss() {
  return wss;
}

module.exports = { initWsServer, getWss };
