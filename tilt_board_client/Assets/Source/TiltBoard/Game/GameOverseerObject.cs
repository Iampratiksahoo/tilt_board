using System;
using System.Collections;
using System.Collections.Generic;
using Source.TiltBoard.Ball;
using Source.TiltBoard.Ball.Util;
using Source.TiltBoard.Board;
using Source.TiltBoard.Game.Signal;
using Source.TiltBoard.Global;
using Source.TiltBoard.Player.Enum;
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
        private PoolController _poolController = null;
        private SignalController _signalController = null;

        private BoardObject _boardObject = null;
        private List<BallObject> _balls = new List<BallObject>();
        private Dictionary<EPlayerType, int> _playerToBallMap = null;

        void Awake()
        {
            _gameConfig = TBConfigUtility.LoadConfiguration<GameConfiguration>("GameConfiguration");
            _signalController = GameManager.Instance.GetController<SignalController>();
            _poolController = GameManager.Instance.GetController<PoolController>();
        }

        void Start()
        {
            // first reset the score
            resetBallCounts();

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
            // first update the score the respective player
            updateScoreByDelta(ballObject.OwnerType, 1);

            // remove it from the list
            _balls.Remove(ballObject);

            // return this guy back to the pool, this will disable the ball
            _poolController.ReturnToPool("Ball", ballObject.gameObject);

            // check if the game is over
            checkGameOver();
        }

        private void checkGameOver()
        {
            // the winner player type
            EPlayerType winnerPlayerType = EPlayerType.None;
            int score = 0;

            // check if any local player has reached the max ball per player score
            if (_playerToBallMap[EPlayerType.Local] >= _gameConfig.BallCountPerPlayer)
            {
                winnerPlayerType = EPlayerType.Local;
                score = _playerToBallMap[EPlayerType.Local];
            }

            // check if any remote player has reached the max ball per player score
            if (_playerToBallMap[EPlayerType.Remote] >= _gameConfig.BallCountPerPlayer)
            {
                winnerPlayerType = EPlayerType.Remote;
                score = _playerToBallMap[EPlayerType.Remote];
            }

            // this means we have a winner
            if (winnerPlayerType != EPlayerType.None)
            {
                // fire the game over signal 
                _signalController.Fire(
                    new GameOverSignal(
                        winnerPlayerType, 
                        score
                    )
                );

                // stop the board and left over ball from moving 
                _boardObject.SetTiltEnabled(false);
                foreach (BallObject ball in _balls)
                {
                    ball.SetRollingActive(false);
                }
            }
        }

        private void resetBallCounts()
        {
            // hard code it to 2 players 
            _playerToBallMap = new Dictionary<EPlayerType, int>()
            {
                { EPlayerType.Local, 0 },
                { EPlayerType.Remote, 0 }
            };
        }

        private void updateScoreByDelta(EPlayerType playerType, int delta)
        {
            // first update the score
            _playerToBallMap[playerType] += delta;

            // send the updated score via signal
            _signalController.Fire(
                new ScoreUpdatedSignal(
                    _playerToBallMap[EPlayerType.Local],
                    _playerToBallMap[EPlayerType.Remote]
                )
            );
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
                EPlayerType thisPlayer = EPlayerType.Local;

                BallObject ball = spawnBall(
                    thisPlayer,
                    _gameConfig.LocalPlayerBallColor,
                    spawnPoint,
                    ballParent.transform
                );

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
                EPlayerType thisPlayer = EPlayerType.Remote;

                BallObject ball = spawnBall(
                    thisPlayer,
                    _gameConfig.RemotePlayerBallColor,
                    spawnPoint,
                    ballParent.transform
                );

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

        private BallObject spawnBall(EPlayerType playerType, EBallType ballType, Transform spawnPoint, Transform ballParent)
        {
            BallObject ball = _poolController.GetFromPool("Ball").GetComponent<BallObject>();
            ball.Initialize(playerType, ballType);
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