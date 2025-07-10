using System;
using UnityEngine;

namespace Source.TiltBoard.Ball
{
    [Serializable]
    public class BallColorToObjectVO
    {
        [SerializeField] private EBallColor ballColor = EBallColor.None;
        [SerializeField] private GameObject ballObject = null;

        /// <summary>
        /// The color for which this VO is responsible
        /// </summary>
        public EBallColor BallColor => ballColor;

        /// <summary>
        /// The ball object associated with the color
        /// </summary>
        public GameObject BallObject => ballObject;  
    }
}