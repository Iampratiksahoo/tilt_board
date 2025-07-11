using System;

namespace Source.TiltBoard.Global
{
    public interface IController
    {
        public void Initialize(Action<bool> onSuccess);
        public void Update(float deltaTime);
        public void Deinitialize();
    }
}