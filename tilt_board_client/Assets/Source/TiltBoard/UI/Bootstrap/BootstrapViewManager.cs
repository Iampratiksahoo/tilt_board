using Source.TiltBoard.Bootstrap;
using TMPro;
using UnityEngine;

namespace Source.TiltBoard.UI.Bootstrap
{
    public class BootstrapViewManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text loadingMessageText = null;
        private Bootstrapper _bootstrapper = null;

        void Update()
        {
            // anyways we are going to have a single instance only
            if (_bootstrapper == null)
            {
                _bootstrapper = FindAnyObjectByType<Bootstrapper>();
            }

            // set the loading message
            if (_bootstrapper != null)
            {
                loadingMessageText.text = _bootstrapper.LoadingMessage;
            }
        }
    }
}