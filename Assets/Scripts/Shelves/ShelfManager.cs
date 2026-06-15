using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Shelves
{
	public class ShelfManager : MonoBehaviour
	{
		private const int NUMBER_OF_SHELVES = 3;

		[SerializeField] private ShelfVisual visualShelfPrefab;

		[SerializeField] private List<ShelfLevelLoadout> loadedShelvesPerLevel = new();

		[Header("Local Animation Positioning")]
		[SerializeField] private Vector3[] shelfPositions = new Vector3[NUMBER_OF_SHELVES];
		[SerializeField] private Vector3 startPosition;
		[SerializeField] private Vector3 endPosition;


        private ShelfLevelLoadout currentShelves;


		private const int NUM_SHELF_VISUALS = NUMBER_OF_SHELVES * 2;
		private ShelfVisual[] visualShelves = new ShelfVisual[NUM_SHELF_VISUALS];

		private int visualShelfIndex = 0;

		private int dataShelfIndex = 0;
		private int dataShelfTotal;

        private void Start()
        {
			LoadNewShift(0);

			//populate visual carrosel
			for(int i = 0; i < NUM_SHELF_VISUALS; ++i)
			{
				var shelf = Instantiate(visualShelfPrefab, transform);

                shelf.ClearShelf();

                visualShelves[i] = shelf;
			}

            LoadNewShift(0);

            RestockShelves();
        }

        private void LoadNewShift(int shift)
		{
			currentShelves = loadedShelvesPerLevel[shift];
			dataShelfTotal = currentShelves.shelvesInShift.Count;
		}

        public void EndShift(int nextShift)
        {
            ClearShelves();

            LoadNewShift(nextShift);

            LoadShelves();
        }

		public void RestockShelves()
		{
            ClearShelves();
            LoadShelves();
		}

		private void ClearShelves()
		{
            //clear current shelves
            int numShelves = NUMBER_OF_SHELVES;

            int clearIndex = visualShelfIndex;

            while (numShelves > 0)
            {
                clearIndex--;

                if (clearIndex < 0) clearIndex = NUM_SHELF_VISUALS - 1;

                var c = visualShelves[clearIndex];
                c.MoveOut(endPosition);

                numShelves--;
            }
        }

		private void LoadShelves()
		{

            //reset count for populating
            int numShelves = NUMBER_OF_SHELVES;

            var totalShelves = currentShelves.shelvesInShift;

            while (numShelves > 0)
            {
                //wrap around the List
                if (dataShelfIndex >= dataShelfTotal) dataShelfIndex = 0;
                if (visualShelfIndex >= NUM_SHELF_VISUALS) visualShelfIndex = 0;

                var shelf = totalShelves[dataShelfIndex];
                var vis = visualShelves[visualShelfIndex];

                vis.LoadShelf(shelf);
                //TODO set better timing spacing
                vis.MoveIn(startPosition, shelfPositions[numShelves - 1], 1);

                numShelves--;
                dataShelfIndex++;
                visualShelfIndex++;
            }
        }
	}
}