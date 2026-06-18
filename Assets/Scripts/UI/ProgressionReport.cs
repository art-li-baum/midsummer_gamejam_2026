using DG.Tweening;
using Gorpozon.WarehouseSim.Data;
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

        private int previousGBucks = 0;
        private int previousRank = 0;
        private int previousLevel = 0;
        private float levelProgress = 0;

        private WaitForSecondsRealtime longDelay;
        private float lerpMultiplier;
        private float lerpDuration;

        private Coroutine activeRoutine;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            ServiceLocator.TryGet(out playerService);
            ServiceLocator.TryGet(out progressionManager);
        }

        private void Update()
        {
            if (activeRoutine == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                longDelay = new WaitForSecondsRealtime(0);
                lerpMultiplier = 0.2f;
                lerpDuration = 0.1f;
            }
        }

        public void ShowProgressionReport()
        {
            continueButton.interactable = false;

            group.blocksRaycasts = true;
            group.interactable = true;
            playerService.SetPause(true);

            var lastLevel = progressionManager.Progression.Levels[previousLevel];
            var nextLevel = progressionManager.Progression.Levels[previousLevel + 1];
            Setup(lastLevel, nextLevel);

            group.DOKill();
            group.DOFade(1, 0.25f).SetUpdate(true).OnComplete(() =>
            {
                activeRoutine = StartCoroutine(CO_ShowProgressionReport());
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
            longDelay = new WaitForSecondsRealtime(0.5f);
            lerpMultiplier = 1;
            float lerpTime = 0;
            lerpDuration = 0;

            int levelGains = (progressionManager.CurrentLevel - previousLevel) + 1;
            ProgressionLevel lastLevel = progressionManager.Progression.Levels[previousLevel];
            ProgressionLevel nextLevel = progressionManager.Progression.Levels[previousLevel + 1];

            yield return longDelay;

            for (int i = 0; i < levelGains; i++)
            {
                yield return null;

                if (i > 0)
                {
                    previousLevel++;
                    levelProgress = 0;
                    previousRank = nextLevel.TotalRequiredGBucks;

                    lastLevel = progressionManager.Progression.Levels[previousLevel];
                    nextLevel = progressionManager.Progression.Levels[previousLevel + 1];
                    Setup(lastLevel, nextLevel);
                }

                float relativeRequirement = nextLevel.TotalRequiredGBucks - lastLevel.TotalRequiredGBucks;
                float relativeStartRank = previousRank - lastLevel.TotalRequiredGBucks;
                float relativeEndRank = progressionManager.TotalRank - lastLevel.TotalRequiredGBucks;
                float startFill = relativeStartRank / relativeRequirement;
                float targetFill = Mathf.Clamp01(relativeEndRank / relativeRequirement);

                float rankLimit = Mathf.Min(progressionManager.TotalRank, nextLevel.TotalRequiredGBucks);

                lerpTime = 0;
                lerpDuration = ((targetFill-startFill) / targetFill) * 0.75f * lerpMultiplier;

                if (lerpDuration <= 0)
                {
                    lerpDuration = 0.01f;
                    longDelay = new WaitForSecondsRealtime(0);
                }

                while (lerpTime <= lerpDuration)
                {
                    lerpTime += Time.unscaledDeltaTime;

                    float progress = lerpTime / lerpDuration;

                    float rank = Mathf.Lerp(previousRank, rankLimit, progress);
                    levelProgress = Mathf.Lerp(startFill, targetFill, progress);

                    currentRankText.text = $"{rank:0}";

                    progressBarFill.fillAmount = levelProgress;

                    yield return null;
                }
            }

            currentFundsText.text = $"$ {progressionManager.CurrentGBucks:0}";

            yield return longDelay;

            previousGBucks = progressionManager.CurrentGBucks;
            previousLevel = progressionManager.CurrentLevel;
            previousRank = progressionManager.TotalRank;

            continueButton.interactable = true;
            activeRoutine = null;
        }

        private void Setup(ProgressionLevel lastLevel, ProgressionLevel nextLevel)
        {
            lastRankText.text = lastLevel.PromotionName;
            nextRankText.text = nextLevel.PromotionName;
            lastThresholdText.text = lastLevel.TotalRequiredGBucks.ToString();
            nextThresholdText.text = nextLevel.TotalRequiredGBucks.ToString();

            currentRankText.text = previousRank.ToString();
            currentFundsText.text = $"$ {previousGBucks}";

            progressBarFill.fillAmount = levelProgress;
        }
    }
}