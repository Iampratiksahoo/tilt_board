using System;
using System.Collections.Generic;
using Source.TiltBoard.Bootstrap;
using Source.TiltBoard.Pool.Util;
using Source.TiltBoard.Util;
using UnityEngine;

namespace Source.TiltBoard.Pool
{
    public class PoolController : IController
    {
        // map to save all the WhiteFrostBehaviour
        private Dictionary<string, Queue<GameObject>> _poolMap = null;

        private PoolConfiguration _poolConfiguration = null;
        private GameObject _poolParentObject = null;

        public string LoadingMessage => "Pre-loading asset...";

        public void Initialize(Action<bool> success)
        {
            // init the new empty map 
            _poolMap = new Dictionary<string, Queue<GameObject>>();

            // create a poolParent, that holds all the pool objects, so the editor stays organised
            _poolParentObject = new GameObject("PoolParent");
            GameObject.DontDestroyOnLoad( _poolParentObject );

            // load the config file 
            _poolConfiguration = TBUtility.LoadConfiguration<PoolConfiguration>("PoolConfiguration");

            // now loop through all the objects and add them to the pool 
            foreach (PoolEntryVO entry in _poolConfiguration.PoolEntries)
            {
                for (int i = 0; i < entry.PrePoolAmount; ++i)
                {
                    addToPool(
                        entry.PoolKey,
                        GameObject.Instantiate(entry.PoolObject, _poolParentObject.transform)
                    );
                }
            }

            success?.Invoke(true);
        }

        public void Update(float deltaTime)
        {
        }

        public void Deinitialize()
        {
            // Destroy all pooled objects
            foreach (KeyValuePair<string, Queue<GameObject>> kvp in _poolMap)
            {
                Queue<GameObject> queue = kvp.Value;
                while (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    if (obj != null)
                    {
                        GameObject.DestroyImmediate(obj);
                    }
                }
            }

            // Destroy the parent container
            if (_poolParentObject != null)
            {
                GameObject.DestroyImmediate(_poolParentObject);
            }

            // Clear the dictionary
            _poolMap.Clear();
        }

        /// <summary>
        /// Tries to get the object from pool if present
        /// Else instantiates and adds to pool 
        /// </summary>
        public GameObject GetFromPool(string poolKey)
        {
            // pool object to retunr 
            GameObject poolObject = null;

            // check if the _poolMap has an entry
            if (_poolMap.ContainsKey(poolKey))
            {
                // check if there is an element in the Queue 
                if (_poolMap[poolKey].Count <= 0)
                {
                    // if no, then create a new one 
                    addToPool(
                        poolKey,
                        GameObject.Instantiate(_poolConfiguration.GetEntryByKey(poolKey).PoolObject, _poolParentObject.transform)
                    );
                }

                // now dequeue and return it
                poolObject = _poolMap[poolKey].Dequeue();

                // set it's parent to null 
                poolObject.transform.parent = null;

                // set it to active 
                poolObject.SetActive(true);
            }
            else
            {
                throw new ArgumentException($"No pool exists for key: {poolKey}");
            }

            return poolObject;
        }

        /// <summary>
        /// Returns an object back to the pool
        public void ReturnToPool(string poolKey, GameObject poolObject)
        {
            // check if the _poolMap has an entry
            if (_poolMap.ContainsKey(poolKey))
            {
                // first set it to invisible 
                poolObject.SetActive(false);

                // now re-parent it to the pool parent 
                poolObject.transform.parent = _poolParentObject.transform;

                // ok, now queue the element to the Queue
                _poolMap[poolKey].Enqueue(poolObject);
            }
            else
            {
                throw new ArgumentException($"No pool exists for key: {poolKey}");
            }
        }

        private void addToPool(string poolKey, GameObject poolObject)
        {
            // first validate if the key is present at all
            if (!_poolMap.ContainsKey(poolKey))
            {
                _poolMap.Add(
                    poolKey,
                    new Queue<GameObject>()
                );
            }

            // first deactivate the object 
            poolObject.SetActive(false);

            // now add the object
            _poolMap[poolKey].Enqueue( poolObject );
        }
    }
}
