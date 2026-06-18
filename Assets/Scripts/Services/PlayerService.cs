using UnityEngine;

namespace Gorpozon.WarehouseSim.Services
{
	public class PlayerService
	{
        public const float MaxSensitivity = 50;

        public bool IsPaused => paused;
		public float MouseSensitivity => mouseSensitivity;

		public bool Frozen = false;

		private bool paused;
		private float mouseSensitivity = 25;

		public void SetPause(bool paused)
		{
			this.paused = paused;
			Frozen = paused;
			SetCursor(paused);
			Time.timeScale = paused ? 0 : 1;
		}

		public void SetCursor(bool active)
		{
			Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = active;
        }

		public void SetMouseSensitivity(float mouseSensitivity)
		{
			this.mouseSensitivity = Mathf.Clamp(mouseSensitivity, 1, MaxSensitivity);
		}
	}
}