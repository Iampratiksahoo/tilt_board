using System;
using System.Collections;
using System.Collections.Generic;
using Source.TiltBoard.Ball;
using Source.TiltBoard.Board;
using Source.TiltBoard.Global;
using Source.TiltBoard.Global.Signal;
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
        private GameCountdownView countdownView = null;

        [Header("Board Animation")]
        [SerializeField] private LeanTweenType boardSpawnEaseType = LeanTweenType.easeOutBounce;
        [SerializeField] private float boardSpawnAnimTime = 1f;

        [Header("Ball Animation")]
        [SerializeField] private float ballSpawnAnimTime = .05f;
        [SerializeField] private LeanTweenType ballSpawnEaseType = LeanTweenType.easeOutBounce;
        [SerializeField] private float timeTillEnemyBallSpawn = .5f;

        private GameConfiguration _gameConfig = null;
        private PoolController _poolController = null;
        private SignalController _signalController = null;

        private BoardObject _boardObject = null;
        private List<BallObject> _balls = new List<BallObject>();

        void Awake()
        {
            _gameConfig = TBConfigUtility.LoadConfiguration<GameConfiguration>("GameConfiguration");
            _signalController = GameManager.Instance.GetController<SignalController>();
            _poolController = GameManager.Instance.GetController<PoolController>();
        }

        void Start()
        {
            spawnBoardAndBall(() =>
            {
                countdownView.StartCountdownFrom(_gameConfig.CountdownBeforeGameStart, () =>
                {
                    _signalController.Fire(
                        new GameStartSignal()
                    );

                    // Todo: Move this to game manager or something else 
                    foreach (BallObject ball in _balls)
                    {
                        ball.SetRollingActive(true); 
                    }
                });
            });
        }

        private void spawnBoardAndBall(Action onComplete)
        {
            // create a parent object for the game elements 
            GameObject gameParent = new GameObject("GameParent");

            _boardObject = _poolController.GetFromPool("Board").GetComponent<BoardObject>();
            _boardObject.transform.position = Vector3.zero;
            _boardObject.transform.localScale = Vector3.zero;
            _boardObject.transform.parent = gameParent.transform;

            LeanTween.scale(
                _boardObject.gameObject,
                Vector3.one,
                boardSpawnAnimTime
            ).setEase(boardSpawnEaseType)
            .setOnComplete(() => StartCoroutine(instantiateBallsCoro(gameParent.transform, onComplete)));
        }

        private IEnumerator instantiateBallsCoro(Transform parent, Action onComplete)
        {
            int ballPerPlayer = _gameConfig.BallCountPerPlayer;
            GameObject ballParent = new GameObject("BallParent");
            ballParent.transform.parent = parent;

            foreach (Transform spawnPoint in _boardObject.PlayerSpawnPointGroup.GetBalancedSpawnPoints(ballPerPlayer))
            {
                bool isDone = false;

                BallObject ball = spawnBall(
                    _gameConfig.LocalPlayerBallColor,
                    spawnPoint,
                    ballParent.transform
                );

                ball.name = "PlayerBall";

                LeanTween.scale(
                    ball.gameObject,
                    Vector3.one,
                    ballSpawnAnimTime
                )
                .setEase(ballSpawnEaseType)
                .setOnComplete(() => isDone = true);
                yield return new WaitUntil(() => isDone);
            }

            yield return new WaitForSeconds(timeTillEnemyBallSpawn);

            foreach (Transform spawnPoint in _boardObject.OpponentSpawnPointGroup.GetBalancedSpawnPoints(ballPerPlayer))
            {
                bool isDone = false;

                BallObject ball = spawnBall(
                    _gameConfig.RemotePlayerBallColor, 
                    spawnPoint,
                    ballParent.transform
                );

                ball.name = "OpponentBall";

                LeanTween.scale(
                    ball.gameObject,
                    Vector3.one,
                    ballSpawnAnimTime
                )
                .setEase(ballSpawnEaseType)
                .setOnComplete(() => isDone = true);
                yield return new WaitUntil(() => isDone);
            }

            // invoke the on Complete 
            onComplete?.Invoke();
        }

        private BallObject spawnBall(EBallColor ballColor, Transform spawnPoint, Transform ballParent)
        {
            BallObject ball = _poolController.GetFromPool("Ball").GetComponent<BallObject>();
            ball.Initialize(ballColor);
            ball.transform.position = spawnPoint.position;
            ball.transform.parent = ballParent.transform;
            ball.gameObject.transform.localScale = Vector3.zero;

            // add this ball to the list 
            _balls.Add(ball);
            return ball; 
        }
    }
}