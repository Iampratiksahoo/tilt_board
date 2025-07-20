using Source.TiltBoard.Player.Enum;

namespace Source.TiltBoard.Game.Signal
{
    public class GameOverSignal
    {
        public EPlayerType WinnerPlayerType { get; private set; } 
        public int Score { get; private set; }

        public GameOverSignal(EPlayerType winnerPlayerType, int score)
        {
            WinnerPlayerType = winnerPlayerType;
            Score = score;
        }  
    }
}