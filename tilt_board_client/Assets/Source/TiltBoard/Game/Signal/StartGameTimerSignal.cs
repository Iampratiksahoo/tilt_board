using System;

namespace Source.TiltBoard.Game.Signal
{
    public class StartGameTimerSignal
    {
        public Action Callback { get; private set; }

        public StartGameTimerSignal(Action callback)
        {
            Callback = callback;
        }
    }
}