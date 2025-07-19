using System.Collections.Generic;
using Source.TiltBoard.Player.Enum;

namespace Source.TiltBoard.Game.Signal
{
    public class ScoreUpdatedSignal
    {
        public int LocalPlayerScore { get; private set; }
        public int RemotePlayerScore { get; private set; }

        public ScoreUpdatedSignal(int localPlayerScore, int remotePlayerScore)
        {
            LocalPlayerScore = localPlayerScore;
            RemotePlayerScore = remotePlayerScore; 
        }
    }
}