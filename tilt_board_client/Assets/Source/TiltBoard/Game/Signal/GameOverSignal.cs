using Source.TiltBoard.Player.Enum;

namespace Source.TiltBoard.Game.Signal
{
    public class GameOverSignal
    {
        public EPlayerType WinnerPlayerType { get; private set; } 

        public GameOverSignal(EPlayerType winnerPlayerType)
        {
            WinnerPlayerType = winnerPlayerType;
        }  
    }
}