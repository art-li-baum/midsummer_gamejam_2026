using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Gorpozon.WarehouseSim.Objects;

namespace Gorpozon.WarehouseSim.Shelves
{
	[CreateAssetMenu(fileName = "NewShelfData", menuName = "Gorpozon/ShelfData")]
	public class ShelfData: ScriptableObject
	{
		public List<GrabbableObject> items;
	}
}