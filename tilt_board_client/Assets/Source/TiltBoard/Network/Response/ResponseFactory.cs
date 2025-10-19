using UnityEngine;

namespace Source.TiltBoard.Network.Response
{
    public class ResponseFactory
    {
        public static ResponseBase GetResponseFromJSON(string json)
        {
            ResponseBase baseRes = parseToResponse<ResponseBase>(json);

            return baseRes.Type switch
            {
                Enum.ERequestType.HANDSHAKE => parseToResponse<HandshakeResponse>(json),
                _ => null
            };
        }

        private static ResponseBase parseToResponse<T>(string json) where T : ResponseBase
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}