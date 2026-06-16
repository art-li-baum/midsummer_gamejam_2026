using SBG.Pocketwatch;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using DG.Tweening;

namespace Gorpozon.WarehouseSim.Shelves
{
    // Contains data for loading and formating items
    public class ShelfVisual: MonoBehaviour
	{
		//TODO: be changed to reference the item SO
		private ShelfData shelfData;

		//TODO: change this to item references?
		private List<GameObject> itemsOnShelf = new();

		private Vector3 ORIGIN = new Vector3(-1.3f, 1.3f, -0.5f);


        public void LoadShelf(ShelfData data)
		{
			shelfData = data;

			int row = 0;
			int col = 0;

			float spacing = 1.3f;

			//TODO: load all the differnt items and sort them
			foreach(var item in shelfData.items)
			{
				if(row > 2)
				{
					row = 0;
					col++;
				}

				var i = GameObject.Instantiate(item, transform);

                //TODO: maybe get spacing data from items themselves
                i.transform.localPosition = new Vector3(spacing * row, spacing * -col) + ORIGIN;

				itemsOnShelf.Add(i);

				row++;
			}

		}

		public void ClearShelf()
		{
			//TODO: unload all generated items in a probably better way
			foreach(var item in itemsOnShelf)
			{
				GameObject.Destroy(item);
			}
			itemsOnShelf.Clear();
		}

		public void MoveIn(Vector3 startPosition, Vector3 position, float timing)
		{
			gameObject.SetActive(true);

			//TODO: move from start to specified position
			transform.localPosition = position;
			transform.DOKill(true);
			transform.DOMove(startPosition, timing).From();
		}
		
		public void MoveOut(Vector3 endPosition)
		{
			//TODO: wait till we've made it to the end
			transform.DOKill(true);
			transform.DOMove(endPosition, 1).OnComplete(() =>
			{
				ClearShelf();
				gameObject.SetActive(false);
			});
	

		}

		//TODO: have a function for removing an item from the shelf
	}
}