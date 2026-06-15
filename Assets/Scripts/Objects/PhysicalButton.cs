using UnityEngine;
using UnityEngine.Events;

namespace Gorpozon.WarehouseSim.Objects
{
    [RequireComponent(typeof(Animator))]
    public class PhysicalButton : MonoBehaviour, IInteractible
    {
        [SerializeField] private UnityEvent OnClick;

        public string InteractionPrompt => "Press Button";

        public bool CanInteract => true;

        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void Interact()
        {
            anim.SetTrigger("Pressed");
            OnClick?.Invoke();
        }
    }
}