using DG.Tweening;
using Gorpozon.WarehouseSim.Services;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class HUD: MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI promptText;
		[SerializeField] private CanvasGroup interactionPrompt;
		[SerializeField] private CanvasGroup inspectControlsGroup;
		[SerializeField] private ShiftReport shiftReport;
		[SerializeField] private float elementFadeTime = 0.2f;
		[SerializeField] private float controlsAlpha = 0.25f;

        private void Awake()
        {
			HideInteractionPrompt();
        }

        public void SetInteractionPrompt(string prompt)
		{
			if (prompt == promptText.text && interactionPrompt.alpha >= controlsAlpha) return;
            
			promptText.text = prompt;

            interactionPrompt.DOKill();
            interactionPrompt.DOFade(controlsAlpha, elementFadeTime);
        }

		public void HideInteractionPrompt()
		{
			if (interactionPrompt.alpha <= 0) return;

            interactionPrompt.DOKill();
            interactionPrompt.DOFade(0, elementFadeTime);
        }

		public void SetInspectControls(bool active)
		{
			inspectControlsGroup.DOKill();
			inspectControlsGroup.DOFade(active ? controlsAlpha : 0, elementFadeTime);
		}

		public void ShowShiftReport(ShiftManager.OrderScore[] scores)
		{
			shiftReport.ShowShiftReport(scores);
		}
	}
}