using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewOrderPool", menuName = "WarehouseSim/OrderPool")]
	public class OrderPool : ScriptableObject
	{
		public ShippingOrder[] Pool;
	}
}