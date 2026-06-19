using UnityEngine;

namespace Gorpozon.WarehouseSim.Data
{
	[CreateAssetMenu(fileName = "NewLevelProgression", menuName = "WarehouseSim/LevelProgression")]
	public class LevelProgression : ScriptableObject
	{
		public ProgressionLevel[] Levels;
	}
}