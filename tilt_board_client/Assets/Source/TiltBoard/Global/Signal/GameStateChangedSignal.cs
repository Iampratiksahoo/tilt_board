namespace Source.TiltBoard.Global.Signal
{
    public class GameStateChangedSignal
    {
        public EGameState LastState { get; private set; }
        public EGameState CurrentState { get; private set; }

        public GameStateChangedSignal(EGameState lastState, EGameState currentState)
        {
            LastState = lastState;
            CurrentState = currentState; 
        }
    }
}