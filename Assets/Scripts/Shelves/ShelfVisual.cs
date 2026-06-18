using SBG.Pocketwatch;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using DG.Tweening;
using Gorpozon.WarehouseSim.Objects;

namespace Gorpozon.WarehouseSim.Shelves
{
    // Contains data for loading and formating items
    public class ShelfVisual: MonoBehaviour
	{
		private ShelfData shelfData;

		private List<ShelfSlot> slots = new();

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


				var i = new GameObject().AddComponent(typeof(ShelfSlot)) as ShelfSlot;
				i.transform.SetParent(transform);
				

                //TODO: maybe get spacing data from items themselves
                i.transform.localPosition = new Vector3(spacing * row, spacing * -col) + ORIGIN;

				slots.Add(i);

				i.Initialize(item);

				row++;


			}

		}

		public void ClearShelf()
		{
			//TODO: unload all generated items in a probably better way
			foreach(var slot in slots)
			{
				slot.ClearItem();
				GameObject.Destroy(slot.gameObject);
			}
			slots.Clear();
		}

		public void MoveIn(Vector3 startPosition, Vector3 position, float timing)
		{
			gameObject.SetActive(true);

			transform.position = position;
			transform.DOKill(true);
			transform.DOMove(startPosition, timing).From();
		}
		
		public void MoveOut(Vector3 endPosition, float time)
		{
			transform.DOKill(true);
			transform.DOMove(endPosition, time).OnComplete(() =>
			{
				ClearShelf();
				gameObject.SetActive(false);
			});
		}
	}
}