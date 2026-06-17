using Gorpozon.WarehouseSim.Data;
using Mono.Cecil;
using SBG.ServiceLocating;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gorpozon.WarehouseSim.Management
{
	public class ProgressionManager
	{
		public event Action<int> OnLevelUp;

        public event Action OnEndOfGame;

		public LevelProgression Progression
		{
			get
			{
				if (progression == null) ServiceLocator.TryGet(out progression);
				return progression;
			}
		}

		public int CurrentGBucks => currentGBucks;
		public int TotalRank => totalRank;
		public int PreviousGBucks => previousGBucks;
		public int PreviousRank => previousRank;
		public int ProgressionLevel => currentEmployeeLevel;
		public int PreviousLevel => previousLevel;

		private LevelProgression progression;
		private int currentEmployeeLevel = 0;
		private int currentGBucks = 0;
		private int totalRank = 0;

		private int previousGBucks = 0;
		private int previousRank = 0;
		private int previousLevel = 0;

        public List<ShippingOrder> GetShiftOrders()
        {
			var currentLevel = Progression.Levels[currentEmployeeLevel];

            var options = currentLevel.OrderPool.ToList();

            List<ShippingOrder> selection = new();

            while (selection.Count < currentLevel.OrdersPerShift && options.Count > 0)
            {
                int index = Random.Range(0, options.Count);
                selection.Add(options[index]);
                options.RemoveAt(index);
            }

            return selection;
        }

        public int EarnGlorpobux(int amount)
		{
			previousGBucks = currentGBucks;
			previousRank = totalRank;

			currentGBucks += amount;
			totalRank += amount;

			EvaluateEmployeeRank();

			return currentGBucks;
		}

		private int EvaluateEmployeeRank()
		{
			if (currentEmployeeLevel + 1 >= Progression.Levels.Count() ||
				totalRank < Progression.Levels[currentEmployeeLevel+1].TotalRequiredGBucks)
			{
                return currentEmployeeLevel;
            }

			previousLevel = currentEmployeeLevel;
			currentEmployeeLevel++;

			OnLevelUp.Invoke(currentEmployeeLevel);

			if (currentEmployeeLevel + 1 >= Progression.Levels.Count()) OnEndOfGame?.Invoke();

			return currentEmployeeLevel;
		}
	}
}