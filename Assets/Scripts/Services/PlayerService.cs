using UnityEngine;

namespace Gorpozon.WarehouseSim.Services
{
	public class PlayerService
	{
		public bool Frozen = false;

		public void SetPause(bool paused)
		{
			Frozen = paused;
			SetCursor(paused);
			Time.timeScale = paused ? 0 : 1;
		}

		public void SetCursor(bool active)
		{
			Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = active;
        }
	}
}