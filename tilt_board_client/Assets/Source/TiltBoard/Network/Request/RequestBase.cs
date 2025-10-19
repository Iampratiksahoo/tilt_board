using System;
using Source.TiltBoard.Network.Enum;

namespace Source.TiltBoard.Network.Request
{
    public interface IRequest
    {
        public ERequestType Type { get; }
    }
}