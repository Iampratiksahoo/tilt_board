using System;
using Source.TiltBoard.Bootstrap;
using Source.TiltBoard.Bootstrap.Signal;
using Source.TiltBoard.Global;
using Source.TiltBoard.Scene.Signal;
using Source.TiltBoard.Signal;
using UnityEngine.SceneManagement;

namespace Source.TiltBoard.Scene
{
    public class SceneController : IController
    {
        public string LoadingMessage => "Loading SceneController...";

        private SignalController _signalController = null;
        private string _currentSceneName = null;

        public void Initialize(Action<bool> onSuccess)
        {
            // set the current scene to boot, because technically it get's loaded there!! 
            _currentSceneName = "boot";

            // cache the reference for the SignalController
            _signalController = GameManager.Instance.GetController<SignalController>();

            // subscribe to bootstrap ready signal
            _signalController.Subscribe<BootstrapLoadedSignal>(onBootstrapLoaded);

            // add a listener to the engines scene change event
            SceneManager.sceneLoaded += onSceneLoaded;

            // invoke success for callback
            onSuccess?.Invoke( true );
        }

        public void Update(float deltaTime) { }

        public void Deinitialize() { }

        /// <summary>
        /// Simply wraps the loadScene method
        /// </summary>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void onSceneLoaded(UnityEngine.SceneManagement.Scene loadedScene, LoadSceneMode loadMode)
        {
            // first fire the scene changed signal
            _signalController.Fire(
                new SceneChangedSignal(
                    _currentSceneName,
                    loadedScene.name
                )
            );

            // now cache the current scene name 
            _currentSceneName = loadedScene.name;
        }

        private void onBootstrapLoaded(BootstrapLoadedSignal signal)
        {
            // once all the controllers are orderly initialized, now load the menu scene 
            SceneManager.LoadScene("main");

            // set the gameState to mainMenu
            GameManager.Instance.GameState = EGameState.MainMenu; 
        }
    }
}