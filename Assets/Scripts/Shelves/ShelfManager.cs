using Gorpozon.WarehouseSim.Management;
using Gorpozon.WarehouseSim.Services;
using NUnit.Framework;
using SBG.Pocketwatch;
using SBG.ServiceLocating;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Shelves
{
	public class ShelfManager : MonoBehaviour
	{
		private const int NUMBER_OF_SHELVES = 3;

		[SerializeField] private ShelfVisual visualShelfPrefab;

		[SerializeField] private List<ShelfLevelLoadout> loadedShelvesPerLevel = new();

		[Header("Local Animation Positioning")]
		[SerializeField] private Transform[] shelfPositions = new Transform[NUMBER_OF_SHELVES];
		[SerializeField] private Transform startPosition;
		[SerializeField] private Transform endPosition;

        [Header("SFX")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip movingSFX;
        [SerializeField] private AudioClip stoppingSFX;

        private ShiftManager shiftManager;
        private ProgressionManager progressionManager;

        private Vector3 shelfOffset;

        private ShelfLevelLoadout currentShelves;

        private const float ROTATE_TIME = 0.75f;


		private const int NUM_SHELF_VISUALS = NUMBER_OF_SHELVES * 2;
		private ShelfVisual[] visualShelves = new ShelfVisual[NUM_SHELF_VISUALS];

		private int visualShelfIndex = 0;

		private int dataShelfIndex = 0;
		private int dataShelfTotal;

        private bool allowRotation = true;

        private void Start()
        {
            ServiceLocator.TryGet(out shiftManager);
            shiftManager.OnShiftBegin += OnStartShift;
            ServiceLocator.TryGet(out progressionManager);

			LoadNewShift(0);

			//populate visual carrosel
			for(int i = 0; i < NUM_SHELF_VISUALS; ++i)
			{
				var shelf = Instantiate(visualShelfPrefab, transform);

                shelf.transform.rotation = transform.rotation;

                shelf.ClearShelf();

                visualShelves[i] = shelf;
			}

            shelfOffset = shelfPositions[0].position - shelfPositions[1].position;
        }

        private void OnStartShift()
        {
            LoadNewShift(progressionManager.CurrentLevel);

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
            if (!shiftManager.ShiftOngoing || !allowRotation) return;

            allowRotation = false;

            ClearShelves();
            LoadShelves();

            audioSource.PlayOneShot(movingSFX);
            this.StartTimer(() => 
            {
                audioSource.Stop();
                audioSource.PlayOneShot(stoppingSFX);

            }, ROTATE_TIME - 0.4f);

            this.StartTimer(() => allowRotation = true, ROTATE_TIME * 2);
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
                c.MoveOut(endPosition.position - (shelfOffset * (NUMBER_OF_SHELVES - numShelves)), ROTATE_TIME * 2 );

                numShelves--;
            }
        }

		private void LoadShelves()
		{

            //reset count for populating
            int i = 0;

            var totalShelves = currentShelves.shelvesInShift;

            while ( i < NUMBER_OF_SHELVES && i < dataShelfTotal)
            {
                //wrap around the List
                if (dataShelfIndex >= dataShelfTotal) dataShelfIndex = 0;
                if (visualShelfIndex >= NUM_SHELF_VISUALS) visualShelfIndex = 0;

                var shelf = totalShelves[dataShelfIndex];
                var vis = visualShelves[visualShelfIndex];

                vis.LoadShelf(shelf);
                //TODO set better timing spacing
                vis.MoveIn(startPosition.position + (shelfOffset * i), shelfPositions[i].position, ROTATE_TIME);

                i++;
                dataShelfIndex++;
                visualShelfIndex++;
            }
        }
	}
}