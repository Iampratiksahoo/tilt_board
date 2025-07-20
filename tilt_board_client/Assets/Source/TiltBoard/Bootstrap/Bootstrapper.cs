using System;
using System.Collections;
using System.Collections.Generic;
using Source.TiltBoard.Bootstrap.Signal;
using Source.TiltBoard.Pool;
using Source.TiltBoard.Scene;
using Source.TiltBoard.Signal;
using UnityEngine;

namespace Source.TiltBoard.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        // controllers that require to manage the game
        private List<IController> _gameControllers = new List<IController>()
        {
            new SignalController(),
            new SceneController(),
            new PoolController()
        };

        // stores data in the map for easier access
        private Dictionary<Type, IController> _gameControllerMap = new Dictionary<Type, IController>();

        // flag to determine if the bootstrapper is initialized
        private bool _isInitialized = false;

        // a text to determine the loading message
        public string LoadingMessage { get; private set; }

        private IEnumerator Start()
        {
            LoadingMessage = "Loading started...";
            yield return StartCoroutine(orderedInitControllers());
        }

        private IEnumerator orderedInitControllers()
        {
            // cleanup the gameControllerMap, as a best practive
            _gameControllerMap.Clear();

            // start initilalizing controllers once after the other
            foreach (IController controller in _gameControllers)
            {
                // flag to wait till we proceed
                bool isInitialized = false;

                // set the message 
                LoadingMessage = string.IsNullOrEmpty(controller.LoadingMessage)
                                        ? "Setting things up..."
                                        : controller.LoadingMessage; 

                // initialize the controller
                controller.Initialize(success => isInitialized = success);

                // save in the map
                _gameControllerMap.Add(controller.GetType(), controller);

                // now wait till the last controller has been initilaized
                yield return new WaitUntil(() => isInitialized);
            }

            // set the initialized flag to true
            _isInitialized = true;

            // once done, set the loading text to done
            LoadingMessage = "Done";

            // once done, fire a bootstrap ready signal
            GetController<SignalController>().Fire(
                new BootstrapLoadedSignal()
            );
        }

        private void Update()
        {
            if (_isInitialized)
            {
                foreach (IController controller in _gameControllers)
                {
                    controller.Update(Time.deltaTime);
                }
            }
        }

        void OnDisable()
        {
            foreach (IController controller in _gameControllers)
            {
                controller.Deinitialize();
            }
        }

        public T GetController<T>() where T : class
        {
            return _gameControllerMap[typeof(T)] as T;
        }
    }
}