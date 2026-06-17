using DG.Tweening;
using Gorpozon.WarehouseSim.Management;
using Gorpozon.WarehouseSim.Services;
using SBG.Pocketwatch;
using SBG.ServiceLocating;
using System;
using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
	public class HUD: MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI promptText;
		[SerializeField] private CanvasGroup promotionPopup;
        [SerializeField] private CanvasGroup interactionPrompt;
		[SerializeField] private CanvasGroup inspectControlsGroup;
		[SerializeField] private ShiftReport shiftReport;
		[SerializeField] private float elementFadeTime = 0.2f;
		[SerializeField] private float controlsAlpha = 0.25f;

		private ProgressionManager progressionManager;

        private void Awake()
        {
			HideInteractionPrompt();
        }

        private void Start()
        {
			ServiceLocator.TryGet(out progressionManager);
			progressionManager.OnLevelUp += ShowPromotionPopup;
        }

        private void OnDestroy()
        {
			progressionManager.OnLevelUp -= ShowPromotionPopup;
        }

        private void ShowPromotionPopup(int lvl)
        {
			this.StartTimer(() =>
			{
				promotionPopup.DOKill();
				promotionPopup.DOFade(1, 0.5f).SetUpdate(false).OnComplete(() =>
				{
					this.StartTimer(() =>
					{
						promotionPopup.DOFade(0, 0.25f);
					}, 4f);
				});

			}, 0.5f);
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