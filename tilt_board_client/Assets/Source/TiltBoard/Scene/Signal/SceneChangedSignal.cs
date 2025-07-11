namespace Source.TiltBoard.Scene.Signal
{
    public class SceneChangedSignal
    {
        public string LastScene { get; private set; }
        public string CurrentScene { get; private set; }

        public SceneChangedSignal(string lastScene, string currentScene)
        {
            LastScene = lastScene;
            CurrentScene = currentScene;
        }
    }
}