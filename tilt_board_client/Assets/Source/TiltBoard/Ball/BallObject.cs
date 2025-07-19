using System.Linq;
using Source.TiltBoard.Ball.Util;
using Source.TiltBoard.Player.Enum;
using Source.TiltBoard.Util;
using UnityEngine;

namespace Source.TiltBoard.Ball
{
    public class BallObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer ballMesh = null;

        private BallConfiguration _config = null;
        private Rigidbody _rigidbody = null;

        /// <summary>
        /// Color this ball Object is associated with 
        /// </summary>
        public EBallType Type { get; private set; }

        /// <summary>
        /// Owener type for this ball, Local or Remote 
        /// </summary>
        public EPlayerType OwnerType { get; private set; }

        void Awake()
        {
            // cache the reference to the Config
            _config = TBConfigUtility.LoadConfiguration<BallConfiguration>("BallConfiguration");

            // cache the rigidbody
            _rigidbody = GetComponentInChildren<Rigidbody>();

            // set the kinematic to true in the beginning 
            _rigidbody.isKinematic = true;
        }

        /// <summary>
        /// This is the initalizer, that initializes the ball, before it can be used.
        /// </summary>
        /// <param name="ballType"></param>
        public void Initialize(EPlayerType ownerType, EBallType ballType)
        {
            // set the owner type
            OwnerType = ownerType; 

            // set the type 
            Type = ballType;

            // look for the ball material by type  
            BallTypeToMaterialVO entryVO = _config.BallToMaterialMap.FirstOrDefault(mat => mat.Type == ballType);

            // check if we got a valid one
            if (entryVO != null
                && entryVO.Material != null)
            {
                // if yes, then set it and move on 
                ballMesh.material = entryVO.Material;
            }
            else
            {
                // else throw error 
                Debug.LogError($"Ball Material for type {ballType} is not set in the BallConfiguration");
            }
        }

        public void SetRollingActive(bool isRollingActive)
        {
            // we set it to kinematic, so it doesn't roll
            _rigidbody.isKinematic = !isRollingActive;
        }
    }
}