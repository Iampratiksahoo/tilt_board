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

        private GameConfiguration _gameConfig = null;
        private SignalController _signalController = null;
        void Awake()
        {
            _gameConfig = TBConfigUtility.LoadConfiguration<GameConfiguration>("GameConfiguration");
            _signalController = GameManager.Instance.GetController<SignalController>();

            _signalController.Subscribe<StartGameCountdownSignal>(onStartGameCountdown);
            _signalController.Subscribe<StartGameTimerSignal>(onStartGameTimer);
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