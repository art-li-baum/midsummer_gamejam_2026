using Gorpozon.WarehouseSim.Objects;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Player
{
	public class InteractionHandler : MonoBehaviour
	{
		[SerializeField] private Transform viewCamera;
		[SerializeField] private float interactionRange = 5f;
		[SerializeField] private LayerMask interactionMask;

		private IInteractible currentTarget;

		void Update()
		{
			if (Physics.Raycast(viewCamera.position, viewCamera.forward, out var hit, interactionRange, interactionMask))
			{
				currentTarget = hit.collider.GetComponent<IInteractible>();
				if (currentTarget != null && currentTarget.CanInteract)
				{
					// TODO: Set UI Prompt
				}
			}
			else
			{
				currentTarget = null;
			}

			if (Input.GetMouseButtonDown(0) && currentTarget != null && currentTarget.CanInteract)
			{
				currentTarget.Interact();
			}
		}

        private void OnDrawGizmosSelected()
        {
			if (viewCamera == null) return;

            Gizmos.color = Color.green;
			Gizmos.DrawRay(viewCamera.position, viewCamera.forward * interactionRange);
        }
    }
}