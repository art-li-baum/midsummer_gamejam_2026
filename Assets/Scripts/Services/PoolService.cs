using Gorpozon.WarehouseSim.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Gorpozon.WarehouseSim.Services
{
	public class PoolService
	{
		private Dictionary<string, ObjectPool<GrabbableObject>> pools = new();

		public GrabbableObject GetProduct(string id, GrabbableObject prefab)
		{
			if (!pools.ContainsKey(id))
			{
				pools.Add(id, CreatePool(prefab));
			}

			return pools[id].Get();
		}

		public void Release(string id, GrabbableObject obj)
		{
			if (!pools.ContainsKey(id)) return;
			pools[id].Release(obj);
		}

		private ObjectPool<GrabbableObject> CreatePool(GrabbableObject prefab)
		{
			return new ObjectPool<GrabbableObject>(() => CreateFunc(prefab), OnGet, OnRelease, OnDestroy);
		}

        private GrabbableObject CreateFunc(GrabbableObject prefab)
        {
			return GameObject.Instantiate(prefab);
        }

        private void OnDestroy(GrabbableObject grabbable)
        {
			if (grabbable == null) return;
			GameObject.Destroy(grabbable.gameObject);
        }

        private void OnRelease(GrabbableObject grabbable)
        {
			if (grabbable == null) return;

			grabbable.transform.SetParent(null);
            grabbable.gameObject.SetActive(false);
        }

        private void OnGet(GrabbableObject grabbable)
        {
            grabbable.gameObject.SetActive(true);
        }
    }
}