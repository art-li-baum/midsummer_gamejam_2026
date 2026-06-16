using UnityEngine;
using System.Collections.Generic;


namespace Gorpozon.WarehouseSim.Shelves
{
	[CreateAssetMenu(fileName = "NewShelfLevelLoadout", menuName = "Gorpozon/ShelfLevelLoadout")]
	public class ShelfLevelLoadout: ScriptableObject
	{
		public List<ShelfData> shelvesInShift;
	}
}