using PlasticGui.WorkspaceWindow.Locks;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabbableObject : MonoBehaviour, IInteractible
    {
        [SerializeField] private float pickupDistance = 2f;
        [SerializeField] private float rotationSpeed = 50f;
        [SerializeField] private float distSpeed = 10f;

        public string InteractionPrompt => isHeld ? "Drop" : "Pick up";

        public bool CanInteract => true;

        private Rigidbody rb;

        private bool isHeld;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!isHeld) return;

            float scrollInput = Input.mouseScrollDelta.y * distSpeed * Time.deltaTime;

            Vector3 rotInput = new Vector3(-Input.GetAxis("Horizontal"),
                                           Input.GetAxis("Vertical"),
                                           -Input.GetAxis("Roll"));

            rotInput *= rotationSpeed * Time.deltaTime;

            Transform camTrans = Camera.main.transform;

            transform.RotateAround(transform.position, camTrans.right, rotInput.y);
            transform.RotateAround(transform.position, camTrans.up, rotInput.x);
            transform.RotateAround(transform.position, camTrans.forward, rotInput.z);

            transform.position += camTrans.forward * scrollInput;
        }

        public void Interact()
        {
            isHeld = !isHeld;
            rb.isKinematic = isHeld;

            if (isHeld)
            {
                Transform camTransform = Camera.main.transform;
                transform.SetParent(camTransform);
                transform.localPosition = Vector3.forward * pickupDistance;
            }
            else
            {
                transform.SetParent(null);
            }
        }
    }
}