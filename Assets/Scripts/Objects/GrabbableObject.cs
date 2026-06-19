using Gorpozon.WarehouseSim.Data;
using Gorpozon.WarehouseSim.Services;
using Gorpozon.WarehouseSim.Shelves;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableObject : MonoBehaviour, IInteractible
    {
        public Product ProductData;

        public string InteractionPrompt => "Pick up";

        public bool CanInteract => !packaged;

        private Rigidbody rb;
        private bool isHeld;
        private bool packaged;
        private bool onShelf;

        private ShelfSlot shelf;

        private ShiftManager shiftManager;
        private PoolService poolService;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            isHeld = false;
            packaged = false;

            if (shiftManager == null) ServiceLocator.TryGet(out shiftManager);
            shiftManager.OnShiftComplete += OnShiftComplete;
        }

        private void OnDisable()
        {
            if (shiftManager != null) shiftManager.OnShiftComplete -= OnShiftComplete;
        }

        private void Start()
        {
            ServiceLocator.TryGet(out poolService);
            rb.linearDamping = 0.1f;
            rb.angularDamping = 2.0f;
        }

        public void Interact()
        {
            isHeld = !isHeld;
            rb.useGravity = !isHeld;

            if (onShelf) RemoveFromShelf();
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

        public void FreezeForShipping(Transform pacakge)
        {
            rb.isKinematic = true;
            isHeld = false;
            packaged = true;
            transform.SetParent(pacakge);
        }

        public void HangOnShelf(ShelfSlot s)
        {
            shelf = s;
            onShelf = true;

            rb.isKinematic = true;
            transform.SetParent(shelf.transform);
            transform.position = shelf.transform.position;
        }

        public void RemoveFromShelf()
        {
            onShelf = false;

            shelf.ItemRemoved();

            rb.isKinematic = false;
            transform.SetParent(null);
        }

        private void OnShiftComplete()
        {
            if (onShelf || packaged) return;

            poolService.Release(ProductData.name, this);
        }
    }
}