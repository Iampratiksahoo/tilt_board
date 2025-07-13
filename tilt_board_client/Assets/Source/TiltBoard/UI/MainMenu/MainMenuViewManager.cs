using Source.TiltBoard.Global;
using Source.TiltBoard.Global.Signal;
using Source.TiltBoard.Signal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.TiltBoard.UI.MainMenu
{
    public class MainMenuViewManager : MonoBehaviour
    {
        [SerializeField]
        private Button startGameButton = null;

        private SignalController _signalController = null; 

        void Awake()
        {
            _signalController = GameManager.Instance.GetController<SignalController>();
        }

        void OnEnable()
        {
            startGameButton.onClick.AddListener(onStartGame);
        }

        void OnDisable()
        {
            startGameButton.onClick.RemoveAllListeners();
        }

        private void onStartGame()
        {
            // fire the game start signal 
            _signalController.Fire(
                new GameStartSignal()
            );

            // then load the game scene 
            SceneManager.LoadScene("game");
        }
    }
}