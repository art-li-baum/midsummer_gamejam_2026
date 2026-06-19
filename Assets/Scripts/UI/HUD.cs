using DG.Tweening;
using Gorpozon.WarehouseSim.Management;
using Gorpozon.WarehouseSim.Services;
using log4net.Core;
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
		[SerializeField] private CanvasGroup startMenu;
		[SerializeField] private CanvasGroup pauseMenu;
		[SerializeField] private CanvasGroup endScreen;
        [SerializeField] private CanvasGroup inGameGroup;
        [SerializeField] private CanvasGroup promotionPopup;
        [SerializeField] private CanvasGroup interactionPrompt;
		[SerializeField] private CanvasGroup inspectControlsGroup;
		[SerializeField] private ShiftReport shiftReport;
		[SerializeField] private float elementFadeTime = 0.2f;
		[SerializeField] private float controlsAlpha = 0.25f;

		private ProgressionManager progressionManager;
		private PlayerService playerService;

		private bool pauseMenuActive;

        private void Awake()
        {
			HideInteractionPrompt();
        }

        private void Start()
        {
			ServiceLocator.TryGet(out progressionManager);
			ServiceLocator.TryGet(out playerService);

            playerService.SetPause(true);

            progressionManager.OnLevelUp += ShowPromotionPopup;
            progressionManager.OnEndOfGame += ShowEndScreen;
        }

        private void Update()
        {
			if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
        }

        private void OnDestroy()
        {
			progressionManager.OnLevelUp -= ShowPromotionPopup;
            progressionManager.OnEndOfGame -= ShowEndScreen;
        }

        public void StartGame()
		{
			startMenu.DOKill();
			inGameGroup.DOKill();

			startMenu.DOFade(0, 2);
			inGameGroup.DOFade(1, 2);

			startMenu.interactable = false;
			startMenu.blocksRaycasts = false;

            playerService.SetPause(false);
        }

		public void TogglePause()
		{
			if (!pauseMenuActive && playerService.IsPaused) return;

			pauseMenuActive = !pauseMenuActive;

			float target = pauseMenuActive ? 1 : 0;

			pauseMenu.interactable = pauseMenuActive;
			pauseMenu.blocksRaycasts = pauseMenuActive;
			playerService.SetPause(pauseMenuActive);

			pauseMenu.DOKill();
			pauseMenu.DOFade(target, 0.2f).SetUpdate(true);
		}

        private void ShowEndScreen()
        {
            endScreen.alpha = 1;
			endScreen.interactable = true;
			endScreen.blocksRaycasts = true;
        }

        private void ShowPromotionPopup(int lvl)
        {
			if (lvl == progressionManager.Progression.Levels.Length - 1) return;

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
            interactionPrompt.DOFade(controlsAlpha, elementFadeTime).SetUpdate(true);
        }

		public void HideInteractionPrompt()
		{
			if (interactionPrompt.alpha <= 0) return;

            interactionPrompt.DOKill();
            interactionPrompt.DOFade(0, elementFadeTime).SetUpdate(true);
        }

		public void SetInspectControls(bool active)
		{
			inspectControlsGroup.DOKill();
			inspectControlsGroup.DOFade(active ? controlsAlpha : 0, elementFadeTime).SetUpdate(true);
		}

		public void ShowShiftReport(ShiftManager.OrderScore[] scores)
		{
			shiftReport.ShowShiftReport(scores);
		}
	}
}