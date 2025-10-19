
using UnityEngine;

namespace Source.TiltBoard.Network.Response
{
    public class HandshakeResponse : ResponseBase
    {
        [SerializeField]
        private bool success;

        [SerializeField]
        private string clientId;

        /// <summary>
        /// Is the server handshake success or failure? 
        /// </summary>
        public bool Success => success;

        /// <summary>
        /// The new client ID assigned by the server 
        /// </summary>
        public string ClientId => clientId;
    }
}