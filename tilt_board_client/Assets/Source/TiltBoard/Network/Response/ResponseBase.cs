using System;
using Source.TiltBoard.Network.Enum;
using UnityEngine;

namespace Source.TiltBoard.Network.Response
{
    [Serializable]
    public class ResponseBase
    {
        [SerializeField]
        private string type;

        public ERequestType Type => System.Enum.Parse<ERequestType>(type);
    }
}