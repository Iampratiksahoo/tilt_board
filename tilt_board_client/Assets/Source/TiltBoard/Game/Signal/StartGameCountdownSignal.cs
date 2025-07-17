using System;

namespace Source.TiltBoard.Game.Signal
{
    public class StartGameCountdownSignal
    {
        public Action Callback { get; private set; }

        public StartGameCountdownSignal(Action callback)
        {
            Callback = callback;
        }
    }
}