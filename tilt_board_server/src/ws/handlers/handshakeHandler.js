const clientService = require("../../services/client/clientService");
const logger = require("../../util/logger");

function handle(ws, msg) 
{
    // response obj with default failure values 
    const response = {
        success: false, 
        clientId: ""
    }

    try
    {
        // this is the first contact between the server and client
        // so register the client 
        const clientId = clientService.register();

        // now that we made it to this point
        // update the response 
        response.success = true;
        response.clientId = clientId; 

        logger.info(`Client registered successfully: ${clientId}`);
    }
    catch(err)
    {
        // some error occured, so don't update the response object
        // the client needs to handle the error in it's own way!
        logger.error("Error occured during the client handshake: ", err);
    }

    // send the response
    ws.send( JSON.stringify( response ));
}

module.exports = { handle };