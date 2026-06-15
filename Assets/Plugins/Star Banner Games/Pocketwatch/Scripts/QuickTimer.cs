using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBG.Pocketwatch
{
	public static class QuickTimer
	{
        #region PUBLIC FUNCTIONS

        /// <summary>
        /// Immediately starts a timer.
        /// Use "this.StartTimer" within a MonoBehaviour so you do not have to assign the Source Behaviour.
        /// </summary>
        /// <param name="sourceBehaviour">The MonoBehaviour that will start the timer's Coroutine</param>
        /// <param name="callback">Will be called at the end of the timer</param>
        /// <param name="unscaledTime">If true, the timer will be unaffected by timescale</param>
        /// <param name="isRepeating">If true, the timer will repeat until it is manually stopped. The Callback will be called at the end of each iteration.</param>
        public static TimerData StartTimer(this MonoBehaviour sourceBehaviour, Action callback, float durationInSeconds = 1f, bool unscaledTime = false, bool isRepeating = false)
        {
            TimerData timer = new TimerData(sourceBehaviour, durationInSeconds, callback, unscaledTime, isRepeating);
            Coroutine timerRoutine = sourceBehaviour.StartCoroutine(ExecuteTimer(timer));
            timer.SetRoutine(timerRoutine);
            return timer;
        }

        /// <summary>
        /// Aborts the assigned timer. The callback will not be invoked.
        /// </summary>
        public static void AbortTimer(TimerData timer)
        {
            if (timer == null || timer.TimerRoutine == null) return;

            timer.SourceBehaviour.StopCoroutine(timer.TimerRoutine);
        }

        #endregion

        #region PRIVATE FUNCTIONS

        private static IEnumerator ExecuteTimer(TimerData timer)
        {
            do
            {
                timer.SetStartTime();

                if (timer.UsingUnscaledTime)
                {
                    yield return new WaitForSecondsRealtime(timer.DurationInSeconds);
                }
                else
                {
                    yield return new WaitForSeconds(timer.DurationInSeconds);
                }

                if (timer.Callback != null)
                {
                    timer.Callback.Invoke();
                }
                else
                {
                    Debug.LogWarning("POCKETWATCH: Timer Callback cannot be invoked because it is NULL");
                }

            } while (timer.IsRepeating);
        }

        #endregion
    }
}