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
			penaltyText.text = "-";
			scoreText.text = "-";
			gBucksText.text = "-";

            accuracyText.text = "[";

            for (int i = 0; i < score.PossibleHits; i++)
			{
				accuracyText.text += "-";
			}

            accuracyText.text += "]";
        }

        public void RevealNextAccuracy()
		{
			revealedAccuracy++;
			accuracyText.text = "[";

            for (int i = 0; i < score.EvalCount; i++)
			{
                if (i == score.PossibleHits) accuracyText.text += "] ";

                if (i >= revealedAccuracy)
				{
					if (i < score.PossibleHits) accuracyText.text += "-";
					continue;
                }

                if (score.Hits > i) accuracyText.text += "<color=green>O</color>";
                else accuracyText.text += "<color=red>X</color>";
            }

			if (score.EvalCount <= score.PossibleHits) accuracyText.text += "]";
        }

		public void LerpPenalty(float progress)
		{
            float current = Mathf.Lerp(0, score.Penalty * 100, progress);
            penaltyText.text = $"- {current:0} %";
        }

		public void LerpScore(float progress)
		{
			float current = Mathf.Lerp(0, score.Percentage*100, progress);
			scoreText.text = $"{current:0} %";
		}

		public void RevealGBucks()
		{
			gBucksText.text = $"$ {score.GBuckReward}";
		}
	}
}