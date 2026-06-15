using Gorpozon.WarehouseSim.Objects;
using Gorpozon.WarehouseSim.UI;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Player
{
	public class InteractionHandler : MonoBehaviour
	{
		[SerializeField] private Transform viewCamera;
		[SerializeField] private float interactionRange = 5f;
		[SerializeField] private LayerMask interactionMask;
        [SerializeField] private float objectGrabForce = 65;
        [SerializeField] private float objectRotationSpeed = 20f;
        [SerializeField] private float distanceAdjustSpeed = 5f;
        [SerializeField] private float defaultHoldDistance = 2f;
        [SerializeField] private float minHoldDistance = 0.5f;
        [SerializeField] private float maxHoldDistance = 4f;

        private GrabbableObject heldObject;
        private float currentHoldDistance;

		private IInteractible currentTarget;
		private HUD hud;

        private void Start()
        {
			ServiceLocator.TryGet(out hud);
        }

        void Update()
		{
            if (heldObject != null) UpdateGrabbing();
            else UpdateInteractions();
		}

        private void UpdateGrabbing()
        {
            float scrollInput = Input.mouseScrollDelta.y * distanceAdjustSpeed * Time.deltaTime;

            Vector3 rotInput = new Vector3(-Input.GetAxis("Horizontal"),
                                           Input.GetAxis("Vertical"),
                                           -Input.GetAxis("Roll"));

            rotInput *= objectRotationSpeed * Time.deltaTime;

            heldObject.AddTorque(viewCamera.right, rotInput.y);
            heldObject.AddTorque(viewCamera.up, rotInput.x);
            heldObject.AddTorque(viewCamera.forward, rotInput.z);

            currentHoldDistance = Mathf.Clamp(currentHoldDistance+scrollInput, minHoldDistance, maxHoldDistance);

            Vector3 targetPos = viewCamera.transform.position + (viewCamera.forward * currentHoldDistance);
            heldObject.MoveTowardsTarget(targetPos, objectGrabForce);

            if (Input.GetMouseButtonDown(0))
            {
                heldObject.Interact();
                heldObject = null;
                hud.SetInspectControls(false);
            }
        }

		private void UpdateInteractions()
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
                hud.HideInteractionPrompt();
                currentTarget = null;
            }

            if (Input.GetMouseButtonDown(0) && currentTarget != null && currentTarget.CanInteract)
            {
                if (currentTarget is GrabbableObject)
                {
                    heldObject = currentTarget as GrabbableObject;
                    currentHoldDistance = defaultHoldDistance;
                    hud.SetInteractionPrompt("Drop");
                    hud.SetInspectControls(true);
                }
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