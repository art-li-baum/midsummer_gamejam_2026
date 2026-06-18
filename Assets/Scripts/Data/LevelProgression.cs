using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewLevelProgression", menuName = "WarehouseSim/LevelProgression")]
	public class LevelProgression : ScriptableObject
	{
		public float PayoutPerCorrectOrder = 5;
		public ProgressionLevel[] Levels;
	}
}