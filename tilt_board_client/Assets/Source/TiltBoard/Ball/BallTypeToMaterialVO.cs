using System;
using UnityEngine;

namespace Source.TiltBoard.Ball
{
    [Serializable]
    public class BallTypeToMaterialVO
    {
        [SerializeField] private EBallType type = EBallType.None;
        [SerializeField] private Material material = null;

        /// <summary>
        /// The Ball Type for which this VO is responsible
        /// </summary>
        public EBallType Type => type;

        /// <summary>
        /// The Material associated with the type
        /// </summary>
        public Material Material => material;  
    }
}