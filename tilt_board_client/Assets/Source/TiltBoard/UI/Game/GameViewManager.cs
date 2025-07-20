using System;
using System.Collections;
using System.Collections.Generic;
using Source.TiltBoard.Ball;
using Source.TiltBoard.Board;
using Source.TiltBoard.Game.Signal;
using Source.TiltBoard.Global;
using Source.TiltBoard.Pool;
using Source.TiltBoard.Signal;
using Source.TiltBoard.Util;
using Source.TiltBoard.Util.Config;
using Unity.VisualScripting;
using UnityEngine;

namespace Source.TiltBoard.UI.Game
{
    public class GameViewManager : MonoBehaviour
    {
        [Header("Countdown")]
        [SerializeField]
        private GameStartCountdownView countdownView = null;

        [Header("Timer")]
        [SerializeField]
        private GameTimerView timerView = null;

        [Header("Game Over")]
        [SerializeField]
        private GameOverView gameOverView = null;

        private GameConfiguration _gameConfig = null;
        private SignalController _signalController = null;
        void Awake()
        {
            _gameConfig = TBConfigUtility.LoadConfiguration<GameConfiguration>("GameConfiguration");
            _signalController = GameManager.Instance.GetController<SignalController>();
        }

        void OnEnable()
        {
            _signalController.Subscribe<StartGameCountdownSignal>(onStartGameCountdown);
            _signalController.Subscribe<StartGameTimerSignal>(onStartGameTimer);
            _signalController.Subscribe<GameOverSignal>(onGameOver);
        }

        void OnDisable()
        {
            _signalController.Unsubscribe<StartGameCountdownSignal>(onStartGameCountdown);
            _signalController.Unsubscribe<StartGameTimerSignal>(onStartGameTimer);
            _signalController.Unsubscribe<GameOverSignal>(onGameOver);   
        }

        private void onGameOver(GameOverSignal signal)
        {
            // stop the timer 
            timerView.SafeStopTimer();

            // then show the name and score 
            gameOverView.Show(
                signal.WinnerPlayerType.ToString(),
                signal.Score
            );
        }

        private void onStartGameCountdown(StartGameCountdownSignal signal)
        {
            countdownView.StartCountdownFrom(_gameConfig.CountdownBeforeGameStart, signal.Callback);
        }

        private void onStartGameTimer(StartGameTimerSignal signal)
        {
            timerView.StartTimer(_gameConfig.MatchDurationInSeconds, signal.Callback);
        }
    }
}