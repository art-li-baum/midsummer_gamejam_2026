using System.Collections.Generic;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewShippingOrder", menuName = "WarehouseSim/ShippingOrder")]
	public class ShippingOrder : ScriptableObject
	{
		[System.Serializable]
		public struct ProductQuantity
		{
			public Product Product;
			[Min(1)]
			public int Amount;
		}

		public List<ProductQuantity> Products;
	}
}