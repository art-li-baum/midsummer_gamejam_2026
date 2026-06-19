using UnityEngine;
using Gorpozon.WarehouseSim.Objects;
using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;

namespace Gorpozon.WarehouseSim.Shelves
{
	public class ShelfSlot: MonoBehaviour
	{

		private GrabbableObject stockedItemPrefab;

		private GrabbableObject instancedItem;

		private bool isStocked = false;

		private PoolService poolService;

		public void Initialize(GrabbableObject obj)
		{
			ServiceLocator.TryGet(out poolService);

			stockedItemPrefab = obj;

			Refresh();
		}
       
		public void Refresh()
		{
			instancedItem = poolService.GetProduct(stockedItemPrefab.ProductData.name, stockedItemPrefab);
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
			if (isStocked) poolService.Release(stockedItemPrefab.ProductData.name, instancedItem);
			instancedItem = null;
			isStocked = false;
		}

        private void OnDisable()
        {
			if (isStocked) poolService.Release(stockedItemPrefab.ProductData.name, instancedItem);
        }
    }
}