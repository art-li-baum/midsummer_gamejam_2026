using Gorpozon.WarehouseSim.Data;
using Gorpozon.WarehouseSim.Management;
using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{ 
	public class ProgressScreen: MonoBehaviour
	{
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI clockedInText;
		[SerializeField] private TextMeshProUGUI ordersText;
		[Space]
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI rankingText;
		[SerializeField] private TextMeshProUGUI gBucksText;
		[SerializeField] private TextMeshProUGUI freedomRankingText;

		private ProgressionManager progressionManager;
		private ShiftManager shiftManager;

        private int currentDay = 1;
        private bool clockedIn;
        private int? totalOrders;
        private int? completedOrders;

        void Start()
		{
			ServiceLocator.TryGet(out progressionManager);
			ServiceLocator.TryGet(out shiftManager);

            RefreshData();

			progressionManager.OnLevelUp += OnLevelUp;
            shiftManager.OnOrderShipped += OnOrderShipped;
            shiftManager.OnShiftComplete += OnShiftComplete;
            shiftManager.OnShiftBegin += OnShiftBegin;
        }

        private void OnDestroy()
        {
			progressionManager.OnLevelUp -= OnLevelUp;
            shiftManager.OnOrderShipped -= OnOrderShipped;
            shiftManager.OnShiftComplete -= OnShiftComplete;
            shiftManager.OnShiftBegin -= OnShiftBegin;
        }

        private void OnShiftComplete()
        {
            clockedIn = false;
            totalOrders = null;
            completedOrders = null;
            currentDay++;
            RefreshData();
        }

        private void OnShiftBegin()
        {
            clockedIn = true;
            totalOrders = shiftManager.QueuedOrderCount;
            completedOrders = 0;
            RefreshData();
        }

        private void OnLevelUp(int lvl)
        {
            RefreshData();
        }

        private void OnOrderShipped()
        {
            completedOrders++;
            RefreshData();
        }

        private void RefreshData()
		{
            var currentLevel = progressionManager.Progression.Levels[progressionManager.CurrentLevel];

            int nextIndex = progressionManager.CurrentLevel + 1;
            if (nextIndex >= progressionManager.Progression.Levels.Length)
            {
                return;
            }

            ProgressionLevel nextLevel = progressionManager.Progression.Levels[nextIndex];

            clockedInText.text = clockedIn ? "YES" : "NO";

            if (totalOrders.HasValue && completedOrders.HasValue)
            {
                ordersText.text = $"{completedOrders.Value} / {totalOrders.Value}";
            }
            else
            {
                ordersText.text = "-";
            }

            dayText.text = $"Day {currentDay}";
            levelText.text = currentLevel.PromotionName;
            rankingText.text = $"{progressionManager.TotalRank} / {nextLevel.TotalRequiredGBucks}";
            gBucksText.text = $"$ {progressionManager.CurrentGBucks}";
            freedomRankingText.text = $"{progressionManager.TotalRank} / {progressionManager.FreedomRank}";
        }
	}
}