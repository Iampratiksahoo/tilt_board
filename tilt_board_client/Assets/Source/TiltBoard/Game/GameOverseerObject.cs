using System;
using System.Collections;
using System.Collections.Generic;
using Source.TiltBoard.Ball;
using Source.TiltBoard.Ball.Util;
using Source.TiltBoard.Board;
using Source.TiltBoard.Game.Signal;
using Source.TiltBoard.Global;
using Source.TiltBoard.Pool;
using Source.TiltBoard.Signal;
using Source.TiltBoard.Util;
using Source.TiltBoard.Util.Config;
using UnityEngine;

namespace Source.TiltBoard.Game
{
    public class GameOverseerObject : MonoBehaviour
    {
        [Header("Board Animation")]
        [SerializeField] private LeanTweenType boardSpawnEaseType = LeanTweenType.easeOutBounce;
        [SerializeField] private float boardSpawnAnimTime = 1f;

        [Header("Ball Animation")]
        [SerializeField] private float ballSpawnAnimTime = .05f;
        [SerializeField] private LeanTweenType ballSpawnEaseType = LeanTweenType.easeOutBounce;
        [SerializeField] private float timeTillEnemyBallSpawn = .5f;

        private GameConfiguration _gameConfig = null;
        private BallConfiguration _ballConfig = null;
        private PoolController _poolController = null;
        private SignalController _signalController = null;

        private BoardObject _boardObject = null;
        private List<BallObject> _balls = new List<BallObject>();

        void Awake()
        {
            _gameConfig = TBConfigUtility.LoadConfiguration<GameConfiguration>("GameConfiguration");
            _ballConfig = TBConfigUtility.LoadConfiguration<BallConfiguration>("BallConfiguration");
            _signalController = GameManager.Instance.GetController<SignalController>();
            _poolController = GameManager.Instance.GetController<PoolController>();
        }

        void Start()
        {
            // first spawn the board and ball 
            spawnBoardAndBall(() =>
            {
                // once done, now fire the signal to start the countdown 
                _signalController.Fire(
                    new StartGameCountdownSignal(onCountdownCompleted)
                );
            });
        }

        private void onCountdownCompleted()
        {
            // once the countdown is done, change the game state to playing 
            GameManager.Instance.GameState = EGameState.Playing;

            // enable the tilt for the board object
            _boardObject.SetTiltEnabled(true);

            // also enable physics in all the object 
            foreach (BallObject ball in _balls)
            {
                ball.SetRollingActive(true);
            }

            // then finally start the countdown 
            _signalController.Fire(
                new StartGameTimerSignal(onGameTimeUp)
            );
        }

        private void onGameTimeUp()
        {
            
        }

        private void onBallCollected(BallObject ballObject)
        {
            // remove it from the list
            _balls.Remove(ballObject);

            // return this guy back to the pool, this will disable the ball
            _poolController.ReturnToPool("Ball", ballObject.gameObject);
        }

        #region BOARD AND BALL SPAWNING
        private void spawnBoardAndBall(Action onComplete)
        {
            // create a parent object for the game elements 
            GameObject gameParent = new GameObject("GameParent");

            _boardObject = _poolController.GetFromPool("Board").GetComponent<BoardObject>();
            _boardObject.transform.position = Vector3.zero;
            _boardObject.transform.localScale = Vector3.zero;
            _boardObject.transform.parent = gameParent.transform;
            _boardObject.BallDetector.OnBallCollected += onBallCollected;

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

        private BallObject spawnBall(EBallType ballType, Transform spawnPoint, Transform ballParent)
        {
            BallObject ball = _poolController.GetFromPool("Ball").GetComponent<BallObject>();
            ball.Initialize(_ballConfig, ballType);
            ball.transform.position = spawnPoint.position;
            ball.transform.parent = ballParent.transform;
            ball.gameObject.transform.localScale = Vector3.zero;

            // add this ball to the list 
            _balls.Add(ball);
            return ball;
        }
        #endregion
    }
}