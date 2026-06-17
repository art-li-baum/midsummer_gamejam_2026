using DG.Tweening;
using Gorpozon.WarehouseSim.Management;
using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gorpozon.WarehouseSim.UI
{
	public class ProgressionReport : MonoBehaviour
	{
        [SerializeField] private Button continueButton;
        [Space]
        [SerializeField] private TextMeshProUGUI lastRankText;
        [SerializeField] private TextMeshProUGUI lastThresholdText;
        [SerializeField] private TextMeshProUGUI nextRankText;
        [SerializeField] private TextMeshProUGUI nextThresholdText;
        [SerializeField] private TextMeshProUGUI currentRankText;
        [SerializeField] private TextMeshProUGUI currentFundsText;
        [SerializeField] private Image progressBarFill;

        private CanvasGroup group;
        private PlayerService playerService;
        private ProgressionManager progressionManager;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            ServiceLocator.TryGet(out playerService);
            ServiceLocator.TryGet(out progressionManager);
        }

        public void ShowProgressionReport()
        {
            continueButton.interactable = false;

            group.blocksRaycasts = true;
            group.interactable = true;
            playerService.SetPause(true);

            var lastLevel = progressionManager.Progression.Levels[progressionManager.PreviousLevel];
            var nextLevel = progressionManager.Progression.Levels[progressionManager.PreviousLevel + 1];
            Setup(lastLevel, nextLevel);

            group.DOKill();
            group.DOFade(1, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                StartCoroutine(CO_ShowProgressionReport());
            });
        }

        public void HideProgressionReport()
        {
            playerService.SetPause(false);
            group.blocksRaycasts = false;
            group.interactable = false;

            group.DOKill();
            group.DOFade(0, 0.25f).SetUpdate(true);
        }

        private IEnumerator CO_ShowProgressionReport()
        {
            var longDelay = new WaitForSecondsRealtime(0.5f);

            var lastLevel = progressionManager.Progression.Levels[progressionManager.PreviousLevel];
            var nextLevel = progressionManager.Progression.Levels[progressionManager.PreviousLevel + 1];

            float lerpTime = 0;
            float lerpDuration = 0.75f;

            float relativeRequirement = nextLevel.TotalRequiredGBucks - lastLevel.TotalRequiredGBucks;
            float relativeStartRank = progressionManager.PreviousRank - lastLevel.TotalRequiredGBucks;
            float relativeEndRank = progressionManager.TotalRank - nextLevel.TotalRequiredGBucks;
            float startFill = relativeStartRank / relativeRequirement;
            float targetFill = relativeEndRank / relativeRequirement;

            yield return longDelay;

            while (lerpTime < lerpDuration)
            {
                lerpTime += Time.unscaledDeltaTime;

                float progress = lerpTime / lerpDuration;

                float rank = Mathf.Lerp(progressionManager.PreviousRank, progressionManager.TotalRank, progress);
                float money = Mathf.Lerp(progressionManager.PreviousGBucks, progressionManager.CurrentGBucks, progress);

                currentRankText.text = $"{rank:0}";
                currentFundsText.text = $"$ {money:0}";

                progressBarFill.fillAmount = Mathf.Lerp(startFill, targetFill, progress);

                yield return null;
            }

            yield return longDelay;

            continueButton.interactable = true;
        }

        private void Setup(Data.ProgressionLevel lastLevel, Data.ProgressionLevel nextLevel)
        {
            lastRankText.text = lastLevel.PromotionName;
            nextRankText.text = nextLevel.PromotionName;
            lastThresholdText.text = lastLevel.TotalRequiredGBucks.ToString();
            nextThresholdText.text = nextLevel.TotalRequiredGBucks.ToString();

            currentRankText.text = progressionManager.PreviousRank.ToString();
            currentFundsText.text = $"$ {progressionManager.PreviousGBucks}";

            float relativeRequirement = nextLevel.TotalRequiredGBucks - lastLevel.TotalRequiredGBucks;
            float relativeRank = progressionManager.PreviousRank - lastLevel.TotalRequiredGBucks;

            float fill = relativeRank / relativeRequirement;
            progressBarFill.fillAmount = Mathf.Lerp(lastLevel.TotalRequiredGBucks, nextLevel.TotalRequiredGBucks, fill);
        }
    }
}