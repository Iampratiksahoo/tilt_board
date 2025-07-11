using System;

namespace Source.TiltBoard.Global
{
    public interface IController
    {
        public string LoadingMessage { get; }
        public void Initialize(Action<bool> onSuccess);
        public void Update(float deltaTime);
        public void Deinitialize();
    }
}