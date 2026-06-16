using DG.Tweening;
using Gorpozon.WarehouseSim.Services;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gorpozon.WarehouseSim.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class ShiftReport: MonoBehaviour
	{
		[SerializeField] private GameObject scoreTotal;
		[SerializeField] private Button continueButton;
		[SerializeField] private OrderScoreEntry scoreEntryPrefab;
		[SerializeField] private RectTransform scoreEntryParent;
		[Space]
		[Header("Total")]
        [SerializeField] private TextMeshProUGUI totalAccuracyText;
        [SerializeField] private TextMeshProUGUI totalPenaltyText;
        [SerializeField] private TextMeshProUGUI totalScoreText;
        [SerializeField] private TextMeshProUGUI totalGbucksText;

        private List<OrderScoreEntry> activeScoreEntries = new();

		private CanvasGroup group;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        public void ShowShiftReport(ShiftManager.OrderScore[] scores)
		{
            Cleanup();

			group.blocksRaycasts = true;
			group.interactable = true;

            group.DOKill();
			group.DOFade(1, 0.25f).OnComplete(() =>
			{
                StartCoroutine(CO_ShowShiftReport(scores));
            });
		}

		private void Cleanup()
		{
            scoreTotal.SetActive(false);
            continueButton.interactable = false;

            for (int i = activeScoreEntries.Count-1; i >= 0; i--)
			{
				Destroy(activeScoreEntries[i].gameObject);
			}

			activeScoreEntries.Clear();
		}

		private IEnumerator CO_ShowShiftReport(ShiftManager.OrderScore[] scores)
		{
			int totalReachedAccuracy = 0;
			int totalAccuracyCount = 0;
			float penaltySum = 0;
			float scoreSum = 0;
			int totalGBucks = 0;

			float lerpTime;
			float lerpDuration;

            var shortDelay = new WaitForSecondsRealtime(0.1f);
			var medDelay = new WaitForSecondsRealtime(0.3f);
			var longDelay = new WaitForSecondsRealtime(0.5f);

            yield return longDelay;

			for (int i = 0; i < scores.Length; i++)
			{
				var score = scores[i];

				var entry = Instantiate(scoreEntryPrefab, scoreEntryParent);
				entry.transform.SetSiblingIndex(scoreEntryParent.childCount - 2);
				entry.Init(i+1, score);

				yield return medDelay;

				totalAccuracyCount += score.ProductScores.Count;

				foreach (var scoreEntry in score.ProductScores)
				{
					yield return shortDelay;
					entry.RevealNextAccuracy();
					totalReachedAccuracy += scoreEntry;
				}

                yield return medDelay;

                penaltySum += score.ExcessPenalty;

                lerpTime = 0;
                lerpDuration = Mathf.Max(score.ExcessPenalty * 0.25f, 0.1f);

                while (lerpTime < lerpDuration)
                {
                    lerpTime += Time.deltaTime;
                    entry.LerpPenalty(lerpTime / lerpDuration);
                    yield return null;
                }

                yield return medDelay;

				scoreSum += score.Percentage;
                lerpTime = 0;
				lerpDuration = Mathf.Max(score.Percentage * 0.25f, 0.1f);

				while (lerpTime < lerpDuration)
				{
					lerpTime += Time.deltaTime;
					entry.LerpScore(lerpTime /  lerpDuration);
					yield return null;
				}

                yield return medDelay;

                totalGBucks += score.GBuckReward;
				entry.RevealGBucks();

				yield return longDelay;
            }

            totalAccuracyText.text = "-";
            totalPenaltyText.text = "-";
            totalScoreText.text = "-";
            totalGbucksText.text = "-";
            scoreTotal.SetActive(true);

            yield return medDelay;
            totalAccuracyText.text = $"{totalReachedAccuracy:0} / {totalAccuracyCount:0}";
            yield return medDelay;

			float avgPenalty = penaltySum / scores.Length;
            lerpTime = 0;
            lerpDuration = Mathf.Max(avgPenalty * 0.25f, 0.1f);

            while (lerpTime < lerpDuration)
            {
                lerpTime += Time.deltaTime;
                float current = Mathf.Lerp(0, avgPenalty * 100, lerpTime / lerpDuration);

                totalPenaltyText.text = $"<color=red>- {avgPenalty * 100} %</color>";
                yield return null;
            }

            yield return medDelay;

			float avgScore = scoreSum / scores.Length;
            lerpTime = 0;
            lerpDuration = Mathf.Max(avgScore * 0.25f, 0.1f);

            while (lerpTime < lerpDuration)
            {
                lerpTime += Time.deltaTime;
                float current = Mathf.Lerp(0, avgScore * 100, lerpTime / lerpDuration);
				
				string color;

				if (current < 25f) color = "red";
				else if (current < 75f) color = "yellow";
				else color = "green";

				totalScoreText.text = $"<color={color}>{current:0} %</color>";
                yield return null;
            }

            yield return medDelay;
			if (totalGBucks > 0) totalGbucksText.text = $"<color=green>+ {totalGBucks}</color>";
			else totalGbucksText.text = $"+ {totalGBucks}";

            yield return longDelay;

			continueButton.interactable = true;
        }
    }
}