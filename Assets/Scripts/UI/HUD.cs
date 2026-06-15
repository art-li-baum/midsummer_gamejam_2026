using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class HUD: MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI interactionPrompt;

        private void Awake()
        {
			SetInteractionPrompt(string.Empty);
        }

        public void SetInteractionPrompt(string prompt)
		{
			if (prompt == interactionPrompt.text) return;
			interactionPrompt.text = prompt;
		}
	}
}