using System.Collections.Generic;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
	[RequireComponent(typeof(Collider))]
	public class Package: MonoBehaviour
	{
        private List<GrabbableObject> heldObjects = new();

        private void OnTriggerEnter(Collider other)
        {
            var grabbable = other.GetComponent<GrabbableObject>();
            if (grabbable != null) heldObjects.Add(grabbable);
        }

        private void OnTriggerExit(Collider other)
        {
            var grabbable = other.GetComponent<GrabbableObject>();
            if (grabbable != null) heldObjects.Remove(grabbable);
        }
    }
}