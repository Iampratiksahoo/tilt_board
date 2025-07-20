const handshakeHandler = require('./handlers/handshakeHandler');
const logger = require("../util/logger");

function handleMessage(ws, data) {
  try 
  {
    // For now assume text JSON messages; we'll migrate to binary later
    const msg = JSON.parse(data);
    logger.info('Received WS message:', msg);

    switch (msg.type) 
    {
      case 'HANDSHAKE':
        handshakeHandler.handle(ws, msg);
        break;

      default:
        logger.info('Unknown WS message type:', msg.type);
    }

  } catch (err) {
    logger.error('Error parsing WS message:', err);
  }
}

module.exports = { handleMessage };
