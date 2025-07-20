using System;
using System.Collections;
using Source.TiltBoard.Util;
using TMPro;
using UnityEngine;

namespace Source.TiltBoard.UI.Game
{
    public class GameStartCountdownView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text countdownText = null;

        [SerializeField]
        private float scaleAnimTime = .1f;

        [SerializeField]
        private LeanTweenType textScaleTweenType = LeanTweenType.easeInSine;

        private Coroutine _countdownCoroutine = null;

        void Awake()
        {
            countdownText.gameObject.SetActive( false );
        }
        
        /// <summary>
        /// Shows the count down in seconds, starting from a value till 0. 
        /// </summary>
        public void StartCountdownFrom(int value, Action onComplete)
        {
            if (_countdownCoroutine != null)
            {
                // cancle all animation for the game 
                LeanTween.cancel(gameObject);

                // stop the coroutine 
                StopCoroutine(_countdownCoroutine);
                
                // now set the scale to 0
                countdownText.transform.localScale = Vector3.zero;

                // now enable the object 
                countdownText.gameObject.SetActive(false);
            }

            _countdownCoroutine = StartCoroutine(startCountdownCoro(value, onComplete));
        }

        private IEnumerator startCountdownCoro(int value, Action onComplete)
        {
            // set the countdown to the value
            int countdown = value;

            // now set the scale to 0
            countdownText.transform.localScale = Vector3.zero;

            // now enable the object 
            countdownText.gameObject.SetActive(true);

            while (countdown >= 0)
            {
                string countdownTxt = countdown <= 0
                                        ? TBTextConstants.CountdownGoMessage
                                        : countdown.ToString();

                // set the text 
                countdownText.text = countdownTxt;

                // now start the animation 
                LeanTween.scale(
                    countdownText.gameObject,
                    Vector3.one,
                    scaleAnimTime
                ).setEase(
                    textScaleTweenType
                );

                yield return new WaitForSeconds(1 - (scaleAnimTime * 2));

                bool animCompleted = false;

                LeanTween.scale(
                    countdownText.gameObject,
                    Vector3.zero,
                    scaleAnimTime
                ).setEase(
                    textScaleTweenType
                ).setOnComplete(() =>
                {
                    animCompleted = true;
                    --countdown;
                });

                yield return new WaitUntil(() => animCompleted);
            }

            // cancle all animation
            LeanTween.cancel(gameObject);

            // once done, invoke the onComplete 
            onComplete?.Invoke();
        }
    }
}