using Gorpozon.WarehouseSim.Data;
using Gorpozon.WarehouseSim.Management;
using Gorpozon.WarehouseSim.Objects;
using Gorpozon.WarehouseSim.UI;
using SBG.ServiceLocating;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gorpozon.WarehouseSim.Services
{
	public class ShiftManager
	{
		[System.Serializable]
		public struct OrderScore
		{
			public int PossibleHits;
			public int EvalCount;
			public int Hits;
			public int Misses;
            public float Percentage;
			public float Penalty;
			public int GBuckReward;

			public OrderScore(int hits, int misses, int possibleHits, float percentage, int gBucks)
			{
				Hits = hits;
				Misses = misses;
				PossibleHits = possibleHits;
				EvalCount = Hits + Misses;
				Percentage = percentage;
				Penalty = 1 - percentage;
				GBuckReward = gBucks;
			}
		}

		public Action<ShippingOrder> OnOrderChanged;
		public Action OnOrderShipped;
		public Action OnShiftComplete;
		public Action OnShiftBegin;

        public ShippingOrder CurrentOrder => currentOrder;
		public int QueuedOrderCount => remainingOrders.Count;
		public List<OrderScore> Scores => orderScores;

		private Queue<ShippingOrder> remainingOrders = new();
		private List<OrderScore> orderScores = new();
		private ShippingOrder currentOrder;

		private ProgressionManager progressionManager;
		private HUD hud;

		private bool shiftOngoing = false;

		public void StartShift()
		{
			if (shiftOngoing) return;

			if (progressionManager == null) ServiceLocator.TryGet(out progressionManager);

            var orders = progressionManager.GetShiftOrders();

            foreach (var order in orders)
			{
                remainingOrders.Enqueue(order);
            }

            orderScores.Clear();

			shiftOngoing = true;
            OnShiftBegin?.Invoke();

            currentOrder = remainingOrders.Dequeue();
			OnOrderChanged?.Invoke(currentOrder);

			GameObject.FindAnyObjectByType<ConveyorBelt>().Next();
		}

		public void FinishOrder(ShippingOrder.ProductQuantity[] shippedProducts)
		{
			if (currentOrder == null) return;

            OnOrderShipped?.Invoke();

			float ratingPercentage = EvaluateOrder(shippedProducts);

            if (remainingOrders.Count > 0)
			{
                currentOrder = remainingOrders.Dequeue();
                OnOrderChanged?.Invoke(currentOrder);
            }
			else
			{
				currentOrder = null;
                OnOrderChanged?.Invoke(null);

				shiftOngoing = false;
                OnShiftComplete?.Invoke();

				int gBucks = orderScores.Select(s => s.GBuckReward).Sum();
				progressionManager.EarnGlorpobux(gBucks);

                if (hud == null) ServiceLocator.TryGet(out hud);
				hud.ShowShiftReport(orderScores.ToArray());
			}
		}

        private float EvaluateOrder(ShippingOrder.ProductQuantity[] shippedProducts)
        {
			int correctItems = 0;
			int mismatchedItems = 0;
			int expectedItems = 0;

			foreach (var requirement in currentOrder.Products)
			{
				expectedItems += requirement.Amount;

                var match = shippedProducts.FirstOrDefault(p => p.Product == requirement.Product);

				if (match.Product == null) continue;

                if (match.Amount >= requirement.Amount)
				{
                    correctItems += requirement.Amount;
					mismatchedItems += match.Amount - requirement.Amount;
                }
                else
                {
					int missedItems = requirement.Amount - match.Amount;
					correctItems += requirement.Amount - missedItems;
                }
            }

			// Find Unique Excess Items (Excess Amount of requested items is checked above)
			foreach (var item in shippedProducts)
			{
				bool isRequested = currentOrder.Products.Any(p => p.Product == item.Product);
				if (!isRequested) mismatchedItems += item.Amount;
			}

			int totalShipped = correctItems + mismatchedItems;
			int missingItems = Mathf.Max(expectedItems - correctItems, 0);
			float score = expectedItems - (mismatchedItems + missingItems);
			float resultPercentage = Mathf.Clamp01(score / expectedItems);
			int gBucks = Mathf.FloorToInt(resultPercentage * progressionManager.Progression.Levels[progressionManager.CurrentLevel].PayoutPerCorrectOrder);

			int misses = Mathf.Max(missingItems, mismatchedItems);

            orderScores.Add(new OrderScore(correctItems, misses, expectedItems, resultPercentage, gBucks));

			return resultPercentage;
        }
    }
}