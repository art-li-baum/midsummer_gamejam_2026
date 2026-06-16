using Gorpozon.WarehouseSim.Data;
using System;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
	public class ConveyorBelt : MonoBehaviour
	{
		public event Action OnPackageShipped;

        [SerializeField] private GameObject boxPrefab_Small;
		[SerializeField] private GameObject boxPrefab_Medium;
		[SerializeField] private GameObject boxPrefab_Large;
		[Space]
        [SerializeField] private float beltSpeed = 5;
		[SerializeField] private int packingStopIndex = 3;
		[SerializeField] private Transform[] path;

		private GameObject currentBox;
		private GameObject nextBox;
		private bool isLerpingBoxes;
		private bool currentBoxArrived;
		private bool nextBoxArrived;
        private int currentBoxPoint;
		private int nextBoxPoint;

        private void FixedUpdate()
        {
			if (!isLerpingBoxes) return;

			if (currentBox != null && !currentBoxArrived)
			{
				Transform p = path[currentBoxPoint];
				Vector3 diff = p.position - currentBox.transform.position;

				if (diff.sqrMagnitude > 0.01f)
				{
					currentBox.transform.position += diff.normalized * Time.fixedDeltaTime * beltSpeed;
                    currentBox.transform.LookAt(p);
                }
                else
				{
					currentBoxPoint++;
					if (currentBoxPoint >= path.Length)
					{
                        currentBoxArrived = true;
						Destroy(currentBox);
						currentBox = null;
                    }
                }
			}


            if (nextBox != null && !nextBoxArrived)
            {
                Transform p = path[nextBoxPoint];
                Vector3 diff = p.position - nextBox.transform.position;

                if (diff.sqrMagnitude > 0.01f)
                {
                    nextBox.transform.position += diff.normalized * Time.fixedDeltaTime * beltSpeed;
					nextBox.transform.LookAt(p);
                }
                else if (nextBoxPoint == packingStopIndex)
                {
					nextBoxArrived = true;
					OnPackageShipped?.Invoke();
                }
            }

			if (currentBoxArrived && nextBoxArrived) isLerpingBoxes = false;
        }

		public void Next() => Next(null);

        public void Next(ShippingOrder nextOrder)
		{
			if (isLerpingBoxes) return;

			currentBox = nextBox;
			currentBoxPoint = nextBoxPoint;

			if (nextOrder != null)
			{
                switch (nextOrder.BoxSize)
                {
                    case ShippingOrder.PackagingSize.Small:
                        nextBox = Instantiate(boxPrefab_Small, path[0].position, path[0].rotation);
                        break;
                    case ShippingOrder.PackagingSize.Medium:
                        nextBox = Instantiate(boxPrefab_Medium, path[0].position, path[0].rotation);
                        break;
                    case ShippingOrder.PackagingSize.Large:
                        nextBox = Instantiate(boxPrefab_Large, path[0].position, path[0].rotation);
                        break;
                    default: break;
                }
            }

			nextBoxPoint = 1;
			isLerpingBoxes = true;
			nextBoxArrived = nextBox != null ? false : true;
			currentBoxArrived = currentBox != null ? false : true;
		}

        private void OnDrawGizmos()
        {
			if (path == null || path.Length < 1) return;

			Gizmos.color = Color.yellow;

			for (int i = 0; i < path.Length; i++)
			{
                Gizmos.color = Color.yellow;

				if (i > 0) Gizmos.DrawLine(path[i - 1].position, path[i].position);

				if (i == 0) Gizmos.color = Color.green;
				else if (i == path.Length - 1) Gizmos.color = Color.red;

				Gizmos.DrawSphere(path[i].position, 0.1f);
			}

			if (path.Length <= packingStopIndex) return;

			Gizmos.color = new Color(1, 1, 0, 0.5f);

            Vector3 pos = path[packingStopIndex].position + Vector3.up * 0.25f;

			Gizmos.matrix = Matrix4x4.TRS(pos, path[packingStopIndex].rotation, new Vector3(1, 0.5f, 1));
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}