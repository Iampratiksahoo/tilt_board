using System;
using System.Threading.Tasks;
using Source.TiltBoard.Bootstrap;
using Source.TiltBoard.Util;
using NativeWebSocket;
using UnityEngine;
using System.Text;
using Source.TiltBoard.Network.Request;
using Source.TiltBoard.Network.Response;
using Source.TiltBoard.Signal;
using Source.TiltBoard.Global;
using Source.TiltBoard.Network.Signal;


namespace Source.TiltBoard.Network
{
    public class NetworkController : IController
    {
        public string LoadingMessage => "Connecting to server...";

        private WebSocket _webSocket = null;
        private bool _isHandshakeSuccessful = false;
        private NetworkConfiguration _networkConfig = null;
        private Action<bool> _onInitializedCallback = null;
        private SignalController _signalController = null;

        public void Initialize(Action<bool> onSuccess)
        {
            // first load the network configuration file 
            _networkConfig = TBConfigUtility.LoadConfiguration<NetworkConfiguration>("NetworkConfiguration");

            // now set the success flag to false
            _isHandshakeSuccessful = false;

            // cache the signal controller
            _signalController = GameManager.Instance.GetController<SignalController>();

            // do the server handshake 
            _ = initializeImpl();
        }

        public void Update(float deltaTime)
        {
#if UNITY_EDITOR
            _webSocket?.DispatchMessageQueue();
#endif
        }

        public void Deinitialize()
        {
            // TODO: Might need to await this!!
            _webSocket.Close();
        }

        private async Task initializeImpl()
        {
            // init the web socket
            _webSocket = new WebSocket(_networkConfig.EndPoint);

            _webSocket.OnOpen += onSocketOpen;
            _webSocket.OnMessage += onSocketMessage;
            _webSocket.OnError += onSocketError; 
            _webSocket.OnClose += onSocketClose; 


            // attempt connect 
            await _webSocket.Connect();
        }

        private void onSocketOpen()
        {
            // shout
            Debug.Log($"Initilization to {_networkConfig.EndPoint} successful");

            // send a handshake request
            SendRequest(
                new HandshakeRequest()
            );
        }

        private void onSocketMessage(byte[] data)
        {
            // Get the encoded string first
            string json = Encoding.UTF8.GetString(data);

            // Then try and parse the json to Response Object 
            ResponseBase reponse = ResponseFactory.GetResponseFromJSON(json);

            // check if valid 
            if (reponse != null)
            {
                // see if this is the handshake response 
                if (reponse is HandshakeResponse hsResponse)
                {
                    // set the handshake flag to appropriate
                    _isHandshakeSuccessful = hsResponse.Success;

                    // fire the initialized signal
                    _onInitializedCallback?.Invoke( _isHandshakeSuccessful );
                }

                // fire the signal only if the handshake is successful 
                if (_isHandshakeSuccessful)
                {
                    _signalController.Fire(
                        new ServerMessageSignal(
                            reponse
                        )
                    );
                }
            }
            else
            {
                Debug.Log($"[Network Controller] Error unable to parse JSON\nPayload: {json}");
            }
        }

        private void onSocketError(string errorMsg)
        {
            Debug.LogError("[Network Controller] Socket Error: " + errorMsg);
        }

        private void onSocketClose(WebSocketCloseCode closeCode)
        {
            Debug.LogError("[Network Controller] Socket Closed with code: " + closeCode);
        }

        public void SendRequest(IRequest request)
        {
            // shoudln't be null 
            if (request != null)
            {
                // ok now shoot
                _webSocket.SendText(
                    JsonUtility.ToJson(
                        request
                    )
                );
            }
        }
    }
}