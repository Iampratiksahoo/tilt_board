using Source.TiltBoard.Bootstrap;
using Source.TiltBoard.Global.Signal;
using Source.TiltBoard.Signal;
using UnityEngine;

namespace Source.TiltBoard.Global
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of this class
        /// </summary>
        public static GameManager Instance { get; private set; }

        private Bootstrapper _bootstrapper = null;

        public EGameState GameState
        {
            get => _gamestate;
            set
            {
                // fire the game state change signal 
                GetController<SignalController>()?.Fire(
                    new GameStateChangedSignal(
                        _gamestate,
                        value
                    )
                );

                // now save the new state
                _gamestate = value;
            }
        }
        private EGameState _gamestate = EGameState.None;

        private void Awake()
        {
            // singleton initalization
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }

            // set the game state to initial load 
            // keep in mind, we set the private variable here, else the property would throw error.
            _gamestate = EGameState.Boot;
        }

        private void Start()
        {
            // create a new gameObject and add the bootstraper component on to it
            GameObject bootstrapObject = new GameObject("Bootstrapper");

            // add the bootstrapper component on to it
            _bootstrapper = bootstrapObject.AddComponent<Bootstrapper>();

            // finally add it to don't destroy
            DontDestroyOnLoad(bootstrapObject);
        }

        /// <summary>
        /// Wraps the bootstrap methods to fetch the controller
        /// </summary>
        public T GetController<T>() where T : class
        {
            return _bootstrapper.GetController<T>();
        }
    }
}