using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewProduct", menuName = "WarehouseSim/Product")]
	public class Product : ScriptableObject
	{
		public string DisplayName;
		[Min(0)]
		public float Price;
		[TextArea]
		public string Description;
	}
}