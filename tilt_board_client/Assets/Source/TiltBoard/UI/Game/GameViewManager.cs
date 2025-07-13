using UnityEngine;

namespace Source.TiltBoard.UI.Game
{
    public class GameViewManager : MonoBehaviour
    {
        [SerializeField]
        private GameCountdownView countdownView = null;

        void Start()
        {
            countdownView.StartCountdownFrom( 5, null );
        }
    }
}