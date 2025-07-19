using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Source.TiltBoard.UI.Game
{
    public class GameTimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText = null;
        [SerializeField] private float heartBeatScaleAmount = 1.1f;
        [SerializeField] private LeanTweenType heartBeatTweenType = LeanTweenType.easeInSine;
        [SerializeField] private float inOutTime = .45f;
        [SerializeField] private Color lastSecondTextColor = Color.red;

        private Coroutine _timerCoroutine = null;
        private string _timeUpText = "Time Up!";
        private Color _originalTextColor = Color.white;

        private void Awake()
        {
            _originalTextColor = timerText.color;
        }

        public void StartTimer(int seconds, Action onTimeUp)
        {
            // first safe stop, if at all required
            SafeStopTimer();

            // then start the timer 
            _timerCoroutine = StartCoroutine(timerCoroutine(seconds, onTimeUp));
        }

        public void SafeStopTimer()
        {
            // if the timer is running
            if (_timerCoroutine != null)
            {   
                // then stop the timer
                StopCoroutine(_timerCoroutine);
            }
        }

        private IEnumerator timerCoroutine(int seconds, Action onTimeUp)
        {
            // get the time span for the start second 
            TimeSpan timeSpan;
            while (seconds >= 0)
            {
                // get the time span for current second
                timeSpan = TimeSpan.FromSeconds(seconds);

                // then show the time in view 
                timerText.text = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

                // if the time is less than 10s, then do a animation
                if (seconds <= 10)
                {
                    startHeartbeatAnimation();
                }

                // then wait for 1 second
                yield return new WaitForSeconds(1f);

                // the decrement the time by 1
                --seconds;
            }

            // change the color to the original color
            timerText.color = _originalTextColor;

            // show the text as time up 
            timerText.text = _timeUpText;

            // once done, invoke time up 
            onTimeUp?.Invoke();
        }

        private void startHeartbeatAnimation()
        {
            Vector3 originalScale = timerText.gameObject.transform.localScale;
            timerText.color = lastSecondTextColor   ;

            LeanTween.scale(
                timerText.gameObject, 
                Vector3.one * heartBeatScaleAmount, 
                inOutTime 
            ).setOnComplete(() =>
            {
                LeanTween.scale(
                    timerText.gameObject,
                    originalScale,
                    inOutTime
                );
            });
        }
    }
}