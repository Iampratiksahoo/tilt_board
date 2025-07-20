using System;
using UnityEngine;

namespace Source.TiltBoard.Pool.Util
{
    [Serializable]
    public class PoolEntryVO
    {
        [Header("The key by which this object is identified")]
        [SerializeField]
        private string poolKey = string.Empty;

        [Header("Object that is required to be contained in the pool")]
        [SerializeField]
        private GameObject poolObject;

        [Header("Amount to pre-pool, to avoid in-game load")]
        [SerializeField]
        private int prePoolAmount = 0;

        /// <summary>
        /// The key by which this object is identified
        /// </summary>
        public string PoolKey => poolKey; 

        /// <summary>
        /// Object that is required to be contained in the pool
        /// </summary>
        public GameObject PoolObject => poolObject;

        /// <summary>
        /// Amount to pre-pool, to avoid in-game load
        /// </summary>
        public int PrePoolAmount => prePoolAmount; 
    }
}