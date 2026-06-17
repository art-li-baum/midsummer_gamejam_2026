using Gorpozon.WarehouseSim.Management;
using SBG.ServiceLocating;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{ 
	public class ProgressScreen: MonoBehaviour
	{
		[SerializeField] private TMP_Text levelAmmount;
        [SerializeField] private TMP_Text glorpobuxAmmount;
		[SerializeField] private TMP_Text packageAmmount;
        [SerializeField] private TMP_Text shiftAmmount;

		private ProgressionManager progressionManager;


        void Start()
		{
			ServiceLocator.TryGet(out progressionManager);

			levelAmmount.text = "1";
			glorpobuxAmmount.text = "0";
			packageAmmount.text = "0";
			shiftAmmount.text = "0";

			progressionManager.OnLevelUp += level => { levelAmmount.text =  (level + 1).ToString(); };
		
		}

	}
}