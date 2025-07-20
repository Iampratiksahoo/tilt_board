const { v4: uuidv4 } = require('uuid');
const client = require("./client");    

class ClientService
{
    constructor()
    {
        // the map that stores all the clients 
        this.clients = new Map();
    }

    register()
    {
        // generate a new uuid 
        const clientId = uuidv4();

        // get the current time 
        const now = Date.now();

        // now add an entry for the new client, and create a new client object 
        this.clients.set(
            clientId,
            new client(
                clientId, 
                now
            )
        );

        // return the clientId
        return clientId
    }

    deregister(clientId)
    {
        return this.clients.delete(clientId);
    }

    get(clientId)
    {
        return this.clients.get(clientId) || null;
    }
}

// export the client service 
module.exports = new ClientService();