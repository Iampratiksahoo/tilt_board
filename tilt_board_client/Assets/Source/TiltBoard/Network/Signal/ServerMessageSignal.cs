using Source.TiltBoard.Network.Response;

namespace Source.TiltBoard.Network.Signal
{
    public class ServerMessageSignal
    {
        public ResponseBase Response { get; private set; }
        public ServerMessageSignal( ResponseBase response )
        {
        }
    }
}
