using System;
using System.Collections;
using System.Collections.Generic;
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

        // controllers that require to manage the game
        private List<IController> _gameControllers = new List<IController>()
        {
            new SignalController()
        };

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
        }

        private IEnumerator Start()
        {
            // ordered init the controllers
            yield return StartCoroutine(orderedInitControllers());
        }

        private IEnumerator orderedInitControllers()
        {
            foreach (IController controller in _gameControllers)
            {
                // flag to wait till we proceed
                bool isInitialized = false;

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