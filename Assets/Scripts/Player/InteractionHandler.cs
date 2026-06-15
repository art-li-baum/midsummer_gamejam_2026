using Gorpozon.WarehouseSim.Objects;
using Gorpozon.WarehouseSim.UI;
using SBG.ServiceLocating;
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
		private HUD hud;

        private void Start()
        {
			ServiceLocator.TryGet(out hud);
        }

        void Update()
		{
			if (Physics.Raycast(viewCamera.position, viewCamera.forward, out var hit, interactionRange, interactionMask))
			{
                currentTarget = hit.collider.GetComponent<IInteractible>();

				if (currentTarget != null && currentTarget.CanInteract)
				{
					hud.SetInteractionPrompt(currentTarget.InteractionPrompt);
				}
			}
			else if (currentTarget != null)
			{
                hud.SetInteractionPrompt(string.Empty);
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