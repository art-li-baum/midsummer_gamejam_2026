using SBG.Pocketwatch;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class ChatMessage: MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI messageText;
		[SerializeField] private TextJitter textJitter;

        public void Setup(string message, float fontSize=60, float jitter=0)
		{
            messageText.text = message;
			messageText.fontSize = fontSize;

			if (jitter > 0)
			{
				// Make sure font size is updated correctly first
				this.StartTimer(() =>
				{
					textJitter.enabled = true;
					textJitter.SetSpeed(jitter);
				}, 0.1f, true);
            }
		}
	}
}