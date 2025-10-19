using UnityEngine;

namespace Source.TiltBoard.Network
{
    public class NetworkConfiguration : ScriptableObject
    {
        [SerializeField] private string url = "localhost";
        [SerializeField] private string port = "8989";

        public string EndPoint
        {
            get
            {
                return "ws://" + url + ":" + port;
            }
        }
    }
}