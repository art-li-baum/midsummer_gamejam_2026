using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gorpozon.WarehouseSim.Management
{
	public class ProgressionManager: MonoBehaviour
	{
		public UnityAction<int> levelUpEvent;

		public UnityAction endOfShiftEvent;

		public UnityAction shiftStartEvent;

		public UnityAction endOfGameEvent;

		private int[] levelProgression = new int[] 
		{
			3,
			8,
			18,
			40,
			85,
			155
		};

		private int currentEmployeeLevel = 0;
		private int currentGlorpobux = 0;
		private int totalGlorpobux = 0;

		public int EarnGlorpobux(int ammount)
		{
			currentGlorpobux += ammount;
			totalGlorpobux += ammount;

			EvaluateEmployeeRank();

			return currentGlorpobux;
		}

		private int EvaluateEmployeeRank()
		{
			if (totalGlorpobux < levelProgression[currentEmployeeLevel]) 
				return currentEmployeeLevel;

			currentEmployeeLevel++;

			levelUpEvent.Invoke(currentEmployeeLevel);

			return currentEmployeeLevel;
		}
	}
}