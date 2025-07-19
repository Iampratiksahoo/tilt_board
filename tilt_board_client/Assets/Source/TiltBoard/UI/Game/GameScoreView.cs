using Source.TiltBoard.Game.Signal;
using Source.TiltBoard.Global;
using Source.TiltBoard.Signal;
using TMPro;
using UnityEngine;

namespace Source.TiltBoard.UI.Game
{
    public class GameScoreView : MonoBehaviour
    {
        [Header("Local")]
        [SerializeField] private TMP_Text localNameText = null;
        [SerializeField] private TMP_Text localScoreText = null;

        [Header("Remote")]
        [SerializeField] private TMP_Text remoteNameText = null;
        [SerializeField] private TMP_Text remoteScoreText = null;

        private SignalController _signalController = null;

        void Awake()
        {
            _signalController = GameManager.Instance.GetController<SignalController>();

            // set the default texts
            // shouldn't matter!! 
            localNameText.text = "P1";
            localScoreText.text = "0";
            remoteNameText.text = "P2";
            remoteScoreText.text = "0";
        }

        void OnEnable()
        {
            _signalController.Subscribe<ScoreUpdatedSignal>(onScoreUpdated);
        }

        void OnDisable()
        {
            _signalController.Unsubscribe<ScoreUpdatedSignal>(onScoreUpdated);
        }

        private void onScoreUpdated(ScoreUpdatedSignal signal)
        {
            localScoreText.text = signal.LocalPlayerScore.ToString();
            remoteScoreText.text = signal.RemotePlayerScore.ToString();
        }
    }
}