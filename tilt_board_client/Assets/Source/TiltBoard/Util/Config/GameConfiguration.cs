using Source.TiltBoard.Ball;
using UnityEngine;

namespace Source.TiltBoard.Util.Config
{
    public class GameConfiguration : ScriptableObject
    {
        [Header("Ball count for each team")]
        [SerializeField]
        private int ballCountPerPlayer = 5;

        [Header("Countdown in seconds after which the game starts")]
        [SerializeField]
        private int countdownBeforeGameStart = 3;

        [Header("Duration of a single match in seconds")]
        [SerializeField]
        private int matchDurationInSeconds = 300;

        [Header("The color of the ball of the local player")]
        [SerializeField]
        private EBallType localPlayerBallColor = EBallType.None;

        [Header("The color of the ball of the remote player")]
        [SerializeField]
        private EBallType remotePlayerBallColor = EBallType.None;


        /// <summary>
        /// Ball count for each team
        /// </summary>
        public int BallCountPerPlayer => ballCountPerPlayer;

        /// <summary>
        /// Countdown in seconds after which the game starts
        /// </summary>
        public int CountdownBeforeGameStart => countdownBeforeGameStart;

        /// <summary>
        /// Duration of a single match in seconds
        /// </summary>
        public int MatchDurationInSeconds => matchDurationInSeconds;        
        
        /// <summary>
        /// The color of the ball of the local player"
        /// </summary>
        public EBallType LocalPlayerBallColor => localPlayerBallColor;

        /// <summary>
        /// The color of the ball of the remote player"
        /// </summary>
        public EBallType RemotePlayerBallColor => remotePlayerBallColor;
    }
}