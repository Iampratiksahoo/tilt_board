using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Source.TiltBoard.Loading
{
    public class LoadingDiceObject : MonoBehaviour
    {
        [SerializeField]
        private List<Sprite> diceImages = null;

        [SerializeField]
        private float imageChangeInterval = .25f;

        [SerializeField]
        private float lastToFirstImageInterval = .5f;

        private Image _image = null;
        private int _currentIndex = 0;
        private Coroutine _cycleCoroutine = null;

        void Awake()
        {
            // fetches and caches the first image 
            _image = GetComponentInChildren<Image>();
        }

        private void OnEnable()
        {
            _currentIndex = 0;
            StartCycling();
        }

        private void OnDisable()
        {
            StopCycling();
        }

        private void StartCycling()
        {
            if (diceImages != null
                && diceImages.Count != 0
                && _image != null)
            {
                _cycleCoroutine = StartCoroutine(CycleSprites());    
            }
        }

        private void StopCycling()
        {
            if (_cycleCoroutine != null)
            {
                StopCoroutine(_cycleCoroutine);
                _cycleCoroutine = null;
            }
        }

        private IEnumerator CycleSprites()
        {
            while (true)
            {
                int lastIndex = _currentIndex;
                _image.sprite = diceImages[_currentIndex];
                _currentIndex = (_currentIndex + 1) % diceImages.Count;

                float waitInterval = (_currentIndex == 0 && lastIndex == diceImages.Count - 1)
                                        ? lastToFirstImageInterval
                                        : imageChangeInterval; 
                yield return new WaitForSeconds(waitInterval);
            }
        }
    }
}