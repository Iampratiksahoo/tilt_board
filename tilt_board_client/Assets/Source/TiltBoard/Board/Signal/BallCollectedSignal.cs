using Source.TiltBoard.Ball;

namespace Source.TiltBoard.Board.Signal
{
    public class BallCollectedSignal
    {
        public BallObject BallObject { get; private set; }

        public BallCollectedSignal(BallObject ballObject)
        {
            BallObject = ballObject;
        }
    }
}