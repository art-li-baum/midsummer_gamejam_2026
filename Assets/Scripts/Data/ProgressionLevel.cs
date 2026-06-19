using Gorpozon.WarehouseSim.UI;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewProgressionLevel", menuName = "WarehouseSim/ProgressionLevel")]
	public class ProgressionLevel : ScriptableObject
	{
        public int TotalRequiredGBucks = 1;
        public float PayoutPerCorrectOrder = 5;
		public int OrdersPerShift = 3;
        public string PromotionName = "Junior Packer";
		public Message[] LevelIntroduction;
		public ShippingOrder[] OrderPool;
    }
}