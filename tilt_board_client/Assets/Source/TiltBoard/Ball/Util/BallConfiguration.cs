using System.Collections.Generic;
using UnityEngine;

namespace Source.TiltBoard.Ball.Util
{
    public class BallConfiguration : ScriptableObject
    {
        [SerializeField] private List<BallTypeToMaterialVO> ballToMaterialMap = null;

        /// <summary>
        /// Map that contains the material mapped to each type 
        /// </summary>
        public List<BallTypeToMaterialVO> BallToMaterialMap => ballToMaterialMap; 
    }
}