using System;
using Source.TiltBoard.Ball;
using Source.TiltBoard.Board.Signal;
using Source.TiltBoard.Global;
using Source.TiltBoard.Signal;
using UnityEngine;

namespace Source.TiltBoard.Board
{
    [RequireComponent(typeof(Collider))]
    public class BallDetectorObject : MonoBehaviour
    {
        public Action<BallObject> OnBallCollected = null;
        private SignalController _signalController = null;

        void Awake()
        {
            // set the collider as trigger, if not already set 
            GetComponent<Collider>().isTrigger = true;

            // cache the reference of the Signal Controller
            _signalController = GameManager.Instance.GetController<SignalController>();
        }

        void OnTriggerEnter(Collider other)
        {   
            if (other.transform.parent.TryGetComponent(out BallObject ballObject))
            {
                // invoke this action, for the objects to know if required 
                OnBallCollected?.Invoke(ballObject);

                // fire the ball collected signal 
                _signalController.Fire(
                    new BallCollectedSignal(
                        ballObject
                    )
                );
            }
        }
    }
}