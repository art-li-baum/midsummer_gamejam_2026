using Gorpozon.WarehouseSim.Services;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class OrderScoreEntry: MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI orderText;
		[SerializeField] private TextMeshProUGUI accuracyText;
		[SerializeField] private TextMeshProUGUI penaltyText;
		[SerializeField] private TextMeshProUGUI scoreText;
		[SerializeField] private TextMeshProUGUI gBucksText;

		private ShiftManager.OrderScore score;
		private int revealedAccuracy = 0;

        public void Init(int orderNr, ShiftManager.OrderScore score)
		{
			this.score = score;

			orderText.text = $"{orderNr:000}";
			accuracyText.text = string.Empty;
			penaltyText.text = "-";
			scoreText.text = "-";
			gBucksText.text = "-";
        }

		public void RevealNextAccuracy()
		{
			revealedAccuracy++;
            accuracyText.text = string.Empty;

            for (int i = 0; i < score.ProductScores.Count; i++)
			{
				if (i >= revealedAccuracy) break;

                if (score.ProductScores[i] >= 1) accuracyText.text += "<color=green>O</color>";
                else accuracyText.text += "<color=red>X</color>";
            }
        }

		public void LerpPenalty(float progress)
		{
            float current = Mathf.Lerp(0, score.ExcessPenalty * 100, progress);
            penaltyText.text = $"- {current:0} %";
        }

		public void LerpScore(float progress)
		{
			float current = Mathf.Lerp(0, score.Percentage*100, progress);
			scoreText.text = $"{current:0} %";
		}

		public void RevealGBucks()
		{
			gBucksText.text = $"+ {score.GBuckReward}";
		}
	}
}