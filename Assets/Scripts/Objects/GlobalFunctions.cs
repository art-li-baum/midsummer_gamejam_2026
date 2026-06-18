using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
	public class GlobalFunctions: MonoBehaviour
	{
		public ShiftManager shiftManager;
		private PlayerService playerService;

        private void Start()
        {
			ServiceLocator.TryGet(out shiftManager);
			ServiceLocator.TryGet(out playerService);
        }

        public void StartShift()
		{
			shiftManager.StartShift();
		}

		public void SetMouseSensitivity(float value)
		{
			playerService.SetMouseSensitivity(value);
		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}
}