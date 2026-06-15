using Gorpozon.WarehouseSim.Data;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableObject : MonoBehaviour, IInteractible
    {
        public Product ProductData;

        public string InteractionPrompt => "Pick up";

        public bool CanInteract => true;

        private Rigidbody rb;
        private bool isHeld;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Interact()
        {
            isHeld = !isHeld;
            rb.useGravity = !isHeld;
        }

        public void MoveTowardsTarget(Vector3 targetPos, float grabForce)
        {
            Vector3 force = (targetPos - transform.position) * grabForce * 10 * Time.fixedDeltaTime;
            rb.linearVelocity = force;
        }

        public void AddTorque(Vector3 axis, float torque)
        {
            rb.AddTorque(axis * torque, ForceMode.VelocityChange);
        }
    }
}