using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.TiltBoard.Ball
{
    public class BallObject : MonoBehaviour
    {
        [SerializeField] private List<BallColorToObjectVO> ballToObjectMap = null;

        private Rigidbody _rigidbody = null;

        /// <summary>
        /// Color this ball Object is associated with 
        /// </summary>
        public EBallColor Color { get; private set; }

        void Awake()
        {
            // disable all the colors as soon as we wake up 
            foreach (BallColorToObjectVO entry in ballToObjectMap)
            {
                // set the object to inactive
                entry.BallObject.SetActive(false);

                // so that the object doesn't start rolling as soon as spawned
                entry.BallObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        /// <summary>
        /// This is the initalizer, that initializes the ball, before it can be used.
        /// </summary>
        /// <param name="ballColor"></param>
        public bool Initialize(EBallColor ballColor)
        {
            // falg to be returned
            bool isInitSuccess = false;

            // set the color 
            Color = ballColor;

            // now enable that particlar gameObject in the child 
            BallColorToObjectVO entryVO = ballToObjectMap.FirstOrDefault(entry => entry.BallColor == Color);

            if (entryVO != null
                && entryVO.BallObject != null)
            {
                // set the object to active 
                entryVO.BallObject.SetActive(true);

                // cache the rigidbody
                _rigidbody = entryVO.BallObject.GetComponent<Rigidbody>();

                // set flag to success 
                isInitSuccess = true;
            }

            return isInitSuccess;
        }

        public void SetRollingActive(bool isRollingActive)
        {
            // we set it to kinematic, so it doesn't roll
            _rigidbody.isKinematic = !isRollingActive;
        }
    }
}