using System;
using System.Collections;
using System.Collections.Generic;
using Source.TiltBoard.Pool;
using Source.TiltBoard.Signal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.TiltBoard.Global
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of this class
        /// </summary>
        public static GameManager Instance { get; private set; }

        // controllers that require to manage the game
        private List<IController> _gameControllers = new List<IController>()
        {
            new SignalController(), 
            new PoolController()
        };


        // TODO: MOVE THIS TO SOMEWHERE ELSE.
        private TMP_Text _loadingMessageViewText = null;

        // stores data in the map for easier access
        private Dictionary<Type, IController> _gameControllerMap = new Dictionary<Type, IController>();

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

            // find and cache the loading message text
            _loadingMessageViewText = GameObject.Find("LoadingMessageView").GetComponent<TMP_Text>();
        }

        private IEnumerator Start()
        {
            // ordered init the controllers
            yield return StartCoroutine(orderedInitControllers());

            // once all the controllers are orderly initialized, now load the menu scene 
            SceneManager.LoadScene("main");
        }

        private void Update()
        {
            foreach (IController controller in _gameControllers)
            {
                controller.Update( Time.deltaTime );
            }
        }

        void OnDisable()
        {
            foreach (IController controller in _gameControllers)
            {
                controller.Deinitialize();
            }
        }

        private IEnumerator orderedInitControllers()
        {
            foreach (IController controller in _gameControllers)
            {
                // flag to wait till we proceed
                bool isInitialized = false;

                // set the loading message
                _loadingMessageViewText.text = controller.LoadingMessage; 

                // initialize the controller
                controller.Initialize(success => isInitialized = success);

                // save in the map
                _gameControllerMap.Add(controller.GetType(), controller);

                // now wait till the last controller has been initilaized
                yield return new WaitUntil(() => isInitialized);
            }
        }

        public T GetController<T>() where T : class
        {
            return _gameControllerMap[typeof(T)] as T;
        }
    }
}