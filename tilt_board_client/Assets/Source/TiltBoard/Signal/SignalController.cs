using System;
using System.Collections.Generic;
using Source.TiltBoard.Global;

namespace Source.TiltBoard.Signal
{
    public class SignalController : IController
    {
        private Dictionary<Type, List<Delegate>> _typeToDelegatesMap = null;

        public void Initialize(Action<bool> success)
        {
            // if the type map is null, then create a new map
            if(_typeToDelegatesMap == null)
            {
                _typeToDelegatesMap = new Dictionary<Type, List<Delegate>>();
            }

            // clear the map irrespective
            _typeToDelegatesMap.Clear();

            success?.Invoke(true);
        }

        public void Update(float deltaTime){}
        public void Deinitialize()
        {
            // clear the contents of the map
            _typeToDelegatesMap?.Clear();
        }

        public void OnGameStart() {}

        /// <summary>
        /// Used to Fire an signal
        /// </summary>
        public void Fire<T>(T signal)
        {
            // the type of signal that was subscribed to
            Type signalType = typeof(T);

            // if there is a list of delegates corresponding to the signalType, then proceed
            if (_typeToDelegatesMap.ContainsKey(signalType))
            {
                foreach (Delegate @delegate in _typeToDelegatesMap[signalType])
                {
                    (@delegate as Action<T>)?.Invoke(signal);
                }
            }
        }

        /// <summary>
        /// Subscribe to an signal type
        /// </summary>
        public void Subscribe<T>(Action<T> listener)
        {
            // the type of signal that was subscribed to
            Type signalType = typeof(T);

            // if the signal type if not already present in the map, then create one
            if (!_typeToDelegatesMap.ContainsKey(signalType))
            {
                _typeToDelegatesMap[signalType] = new List<Delegate>();
            }

            // avoid any duplications of delegates
            if (!_typeToDelegatesMap[signalType].Contains(listener))
            {
                _typeToDelegatesMap[signalType].Add(listener);
            }
        }

        /// <summary>
        /// Unsubscribe from an signal type
        /// </summary>
        public void Unsubscribe<T>(Action<T> listener)
        {
            // the type of signal that was requestec to unsubscribe from
            Type signalType = typeof(T);

            // if the signal type if not already present in the map, then create one
            if (_typeToDelegatesMap.ContainsKey(signalType)
                    && _typeToDelegatesMap[signalType].Contains(listener))
            {
                _typeToDelegatesMap[signalType].Remove(listener);

                // if there are no more delegates left for a type, then remove it from the map
                if(_typeToDelegatesMap[signalType].Count <= 0)
                {
                    _typeToDelegatesMap.Remove(signalType);
                }
            }
        }
    }
}
