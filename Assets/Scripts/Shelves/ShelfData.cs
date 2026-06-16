using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace Gorpozon.WarehouseSim.Shelves
{
	[CreateAssetMenu(fileName = "NewShelfData", menuName = "Gorpozon/ShelfData")]
	public class ShelfData: ScriptableObject
	{
		//TODO: Have this be the Item class list
		public List<GameObject> items;
	}
}