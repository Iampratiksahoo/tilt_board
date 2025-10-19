using System;
using Source.TiltBoard.Network.Enum;

namespace Source.TiltBoard.Network.Request
{
    [Serializable]
    public class HandshakeRequest : IRequest
    {
        public ERequestType Type => ERequestType.HANDSHAKE;
    }
}