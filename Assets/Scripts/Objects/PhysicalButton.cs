using SBG.Pocketwatch;
using UnityEngine;
using UnityEngine.Events;

namespace Gorpozon.WarehouseSim.Objects
{
    [RequireComponent(typeof(Animator))]
    public class PhysicalButton : MonoBehaviour, IInteractible
    {
        [SerializeField] private float cooldown = 0.15f;
        [SerializeField] private UnityEvent OnClick;

        public string InteractionPrompt => "Press Button";

        public bool CanInteract => active;

        private bool active = true;

        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void Interact()
        {
            if (!active) return;

            anim.SetTrigger("Pressed");
            OnClick?.Invoke();

            active = false;

            this.StartTimer(() => { active = true; }, cooldown);
        }
    }
}