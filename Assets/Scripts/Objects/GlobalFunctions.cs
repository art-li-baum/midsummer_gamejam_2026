using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
	public class GlobalFunctions: MonoBehaviour
	{
		public ShiftManager shiftManager;

        private void Start()
        {
			ServiceLocator.TryGet(out shiftManager);
        }

        public void StartShift()
		{
			shiftManager.StartShift();
		}
	}
}