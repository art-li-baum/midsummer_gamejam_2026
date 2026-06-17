using UnityEngine;
using Gorpozon.WarehouseSim.Objects;

namespace Gorpozon.WarehouseSim.Shelves
{
	public class ShelfSlot: MonoBehaviour
	{

		private GrabbableObject stockedItemPrefab;

		private GrabbableObject instancedItem;

		private bool isStocked = false;

		public void Initialize(GrabbableObject obj)
		{
			stockedItemPrefab = obj;

			Refresh();
		}
       
		public void Refresh()
		{
			instancedItem = Instantiate(stockedItemPrefab) as GrabbableObject;
			instancedItem.HangOnShelf(this);

			isStocked = true;
		}

		public void ItemRemoved()
		{
			instancedItem = null;
			isStocked = false;
		}

		public void ClearItem()
		{
			if (isStocked)
				GameObject.Destroy(instancedItem);
			instancedItem = null;
		}
	}
}