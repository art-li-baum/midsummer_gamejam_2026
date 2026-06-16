using Gorpozon.WarehouseSim.Data;
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
			public List<int> ProductScores;
			public float ExcessPenalty;
			public float Percentage;
			public int GBuckReward;

			public OrderScore(List<int> scores, float excessPenalty, float percentage, int gBucks)
			{
				ProductScores = scores;
				ExcessPenalty = excessPenalty;
				Percentage = percentage;
				GBuckReward = gBucks;
			}
		}

		public const int OrdersPerDay = 5;

		public Action<ShippingOrder> OnOrderChanged;
		public Action OnShiftComplete;

        public ShippingOrder CurrentOrder => currentOrder;
		public int QueuedOrderCount => remainingOrders.Count;
		public List<OrderScore> Scores => orderScores;

		private Queue<ShippingOrder> remainingOrders = new();
		private List<OrderScore> orderScores = new();
		private ShippingOrder currentOrder;

		private OrderPool pool;
		private HUD hud;

		public void StartShift()
		{
			if (pool == null) ServiceLocator.TryGet(out pool);

			for (int i = 0; i < OrdersPerDay; i++)
			{
				var order = pool.Pool[Random.Range(0, pool.Pool.Length)];
				remainingOrders.Enqueue(order);
			}

			orderScores.Clear();
			currentOrder = remainingOrders.Dequeue();
			OnOrderChanged?.Invoke(currentOrder);
		}

		public void FinishOrder(ShippingOrder.ProductQuantity[] shippedProducts)
		{
			if (currentOrder == null) return;

			float ratingPercentage = EvaluateOrder(shippedProducts);
			UnityEngine.Debug.Log($"Score: {ratingPercentage} % - (Deduction: {orderScores[^1].ExcessPenalty})");

			if (remainingOrders.Count > 0)
			{
                currentOrder = remainingOrders.Dequeue();
                OnOrderChanged?.Invoke(currentOrder);
            }
			else
			{
				currentOrder = null;
                OnOrderChanged?.Invoke(currentOrder);
                OnShiftComplete?.Invoke();

				if (hud == null) ServiceLocator.TryGet(out hud);
				hud.ShowShiftReport(orderScores.ToArray());
			}
		}

        private float EvaluateOrder(ShippingOrder.ProductQuantity[] shippedProducts)
        {
			List<int> itemScores = new(); // 0 for fail, 1 for success
			int excessProducts = 0;

			foreach (var requirement in currentOrder.Products)
			{
                var match = shippedProducts.FirstOrDefault(p => p.Product == requirement.Product);

				for (int i = 0; i < requirement.Amount; i++)
				{
					if (match.Product == null || match.Amount < requirement.Amount) itemScores.Add(0);
					else itemScores.Add(1);
				}

				int excess = match.Amount - requirement.Amount;
				if (excess > 0) excessProducts += excess;
            }

			foreach (var item in shippedProducts)
			{
				if (!currentOrder.Products.Any(p => p.Product == item.Product))
				{
                    excessProducts += item.Amount;
                }
			}

			float score = 0;
			
			for (int i = 0; i < itemScores.Count; i++) score += itemScores[i];

			float excessPenalty = excessProducts * 0.1f; // 10 % penalty per item
			float resultPercentage = Mathf.Clamp01((score / itemScores.Count) - excessPenalty);
			int gBucks = Mathf.FloorToInt(resultPercentage * 10); // 1 Buck per 10%

            orderScores.Add(new OrderScore(itemScores, excessPenalty, resultPercentage, gBucks));

			return resultPercentage;
        }
    }
}