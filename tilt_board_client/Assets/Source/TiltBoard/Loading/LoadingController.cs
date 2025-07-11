using System;
using Source.TiltBoard.Global;

namespace Source.TiltBoard.Loading
{
    public class LoadingController : IController
    {
        private LoadingDiceObject loadingDiceObject = null;

        public string LoadingMessage => throw new NotImplementedException();

        public void Initialize(Action<bool> onSuccess)
        {
            
        }

        public void Update(float deltaTime)
        {

        }
        
        public void Deinitialize()
        {
            
        }
    }
}