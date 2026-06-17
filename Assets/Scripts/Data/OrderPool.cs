using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewOrderPool", menuName = "WarehouseSim/OrderPool")]
	public class OrderPool : ScriptableObject
	{
		[SerializeField] private ShippingOrder[] pool;

		public List<ShippingOrder> GetSelection(int amount)
		{
			var options = pool.ToList();

			List<ShippingOrder> selection = new();

			while (selection.Count < amount && options.Count > 0)
			{
				int index = Random.Range(0, options.Count);
                selection.Add(options[index]);
                options.RemoveAt(index);
			}

			return selection;
		}
	}
}