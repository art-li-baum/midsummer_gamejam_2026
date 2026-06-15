using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBG.Pocketwatch
{
	public class TimerData
	{
        #region PUBLIC FIELDS

        /// <summary>
        /// The Coroutine that is executing this timer
        /// </summary>
        public Coroutine TimerRoutine { get; private set; }
		/// <summary>
		/// Resets on Iterations of Repeating Timers
		/// </summary>
		public float StartTime { get; private set; }
		public float RemainingSeconds => DurationInSeconds - (CurrentTime - StartTime);
		public float LifetimeInSeconds => CurrentTime - StartTime;

		/// <summary>
		/// The MonoBehaviour that runs the Timer Coroutine
		/// </summary>
		public readonly MonoBehaviour SourceBehaviour;
		public readonly float DurationInSeconds;
		public readonly Action Callback;
		public readonly bool UsingUnscaledTime;
		/// <summary>
		/// If true, the timer will repeat until it is manually stopped.
		/// The Callback will be called at the end of each iteration.
		/// </summary>
		public readonly bool IsRepeating;

        #endregion

        #region PRIVATE FIELDS

		/// <summary>
		/// The current time adjusted to the timers scale
		/// </summary>
        private float CurrentTime
        {
            get
            {
				if (UsingUnscaledTime)
				{
					return Time.unscaledTime;
				}
				else
				{
					return Time.time;
				}
			}
        }

		#endregion

		#region PUBLIC FUNCTIONS

		/// <param name="sourceBehaviour">The MonoBehaviour that runs the timer's coroutine</param>
		/// <param name="durationInSeconds">The timer's duration in seconds</param>
		/// <param name="callback">Will be called at the end of the timer</param>
		/// <param name="unscaledTime">If true, the timer will be unaffected by timescale</param>
		/// <param name="isRepeating">If true, the timer will repeat until it is manually stopped. The Callback will be called at the end of each iteration.</param>
		public TimerData(MonoBehaviour sourceBehaviour, float durationInSeconds, Action callback, bool unscaledTime, bool isRepeating)
		{
			SourceBehaviour = sourceBehaviour;
			DurationInSeconds = durationInSeconds;
			Callback = callback;
			UsingUnscaledTime = unscaledTime;
			IsRepeating = isRepeating;
		}

		/// <summary>
		/// Sets the Coroutine that is executing this timer
		/// </summary>
		/// <param name="timerRoutine">The Coroutine that is executing this timer</param>
		public void SetRoutine(Coroutine timerRoutine)
        {
			TimerRoutine = timerRoutine;
		}

		/// <summary>
		/// Execute this within the coroutine that starts the timer so the TimerData is accurate
		/// </summary>
		public void SetStartTime()
        {
			StartTime = CurrentTime;
		}

        #endregion
    }
}