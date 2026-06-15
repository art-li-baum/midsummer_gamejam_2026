using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBG.Pocketwatch
{
	public class QueueTimer
	{
        #region PUBLIC FIELDS

        /// <summary>
        /// The timer currently running in the Queue.
        /// </summary>
        public TimerData RunningTimer { get; private set; }

        /// <summary>
        /// Number of Timers in the Queue.
        /// </summary>
        public int QueueLength => _queuedTimers != null ? _queuedTimers.Count : 0;

        #endregion

        #region PRIVATE FIELDS

        private Queue<TimerData> _queuedTimers;
        private MonoBehaviour _sourceBehaviour;

        #endregion

        #region PUBLIC FUNCTIONS

        /// <summary>
        /// Instantiates a "QueueTimer" to which you can enqueue timers which will run in the assigned order
        /// </summary>
        /// <param name="sourceBehaviour">The MonoBehaviour on which to run the timer coroutines</param>
        public QueueTimer(MonoBehaviour sourceBehaviour)
        {
            _sourceBehaviour = sourceBehaviour;
            _queuedTimers = new Queue<TimerData>();
        }

        /// <summary>
        /// Will start the specified timer once it reaches the end of the Queue. 
        /// </summary>
        /// <param name="callback">Will be called at the end of the timer</param>
        /// <param name="unscaledTime">If true, the timer will be unaffected by timescale</param>
        /// <param name="isRepeating">If true, the timer will repeat until it is manually stopped. The Callback will be called at the end of each iteration.</param>
        public void EnqueueTimer(Action callback, float durationInSeconds = 1f, bool unscaledTime = false, bool isRepeating = false)
        {
            TimerData timer = new TimerData(_sourceBehaviour, durationInSeconds, callback, unscaledTime, isRepeating);

            if (RunningTimer == null)
            {
                Coroutine timerRoutine = _sourceBehaviour.StartCoroutine(ExecuteTimer(timer));
                timer.SetRoutine(timerRoutine);

                RunningTimer = timer;
            }
            else
            {
                _queuedTimers.Enqueue(timer);
            }
        }

        /// <summary>
        /// Aborts the currently running timer.
        /// If there are more timers in the queue, the next one will be triggered.
        /// </summary>
        public void AbortRunningTimer()
        {
            if (RunningTimer == null || RunningTimer.TimerRoutine == null) return;

            _sourceBehaviour.StopCoroutine(RunningTimer.TimerRoutine);

            ProgressQueue();
        }

        /// <summary>
        /// This will abort the entire queue, including the currently running timer
        /// </summary>
        public void AbortAll()
        {
            if (RunningTimer != null)
            {
                if (RunningTimer.TimerRoutine != null)
                {
                    _sourceBehaviour.StopCoroutine(RunningTimer.TimerRoutine);
                }

                RunningTimer = null;
            }

            if (_queuedTimers.Count > 0)
            {
                _queuedTimers.Clear();
            }
        }

        #endregion

        #region PRIVATE FUNCTIONS

        private void ProgressQueue()
        {
            if (_queuedTimers.Count > 0)
            {
                TimerData newTimer = _queuedTimers.Dequeue();
                Coroutine timerRoutine = _sourceBehaviour.StartCoroutine(ExecuteTimer(newTimer));
                newTimer.SetRoutine(timerRoutine);
                RunningTimer = newTimer;
            }
            else
            {
                RunningTimer = null;
            }
        }

        private IEnumerator ExecuteTimer(TimerData timer)
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

                timer.Callback?.Invoke();

            } while (timer.IsRepeating);

            ProgressQueue();
        }

        #endregion
    }
}