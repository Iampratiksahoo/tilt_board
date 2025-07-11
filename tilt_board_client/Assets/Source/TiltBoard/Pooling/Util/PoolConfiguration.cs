using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.TiltBoard.Pool.Util
{
    public class PoolConfiguration : ScriptableObject
    {
        [SerializeField]
        private List<PoolEntryVO> poolEntries = null;

        /// <summary>
        /// The pool entries from the config file.
        /// </summary>
        public List<PoolEntryVO> PoolEntries => poolEntries;


        public PoolEntryVO GetEntryByKey(string poolKey)
        {
            return poolEntries.FirstOrDefault(entry => entry.PoolKey == poolKey); 
        }
    }
}