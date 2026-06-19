using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewProduct", menuName = "WarehouseSim/Product")]
	public class Product : ScriptableObject
	{
		public string DisplayName;
		public string Brand;
		[Min(0)]
		public float Price;
		[TextArea]
		public string Description;
	}
}