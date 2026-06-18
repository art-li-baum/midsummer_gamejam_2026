using SBG.Pocketwatch;
using UnityEngine;
using UnityEngine.Events;

namespace Gorpozon.WarehouseSim.Objects
{
    [RequireComponent(typeof(Animator))]
    public class PhysicalButton : MonoBehaviour, IInteractible
    {
        [SerializeField] private float cooldown = 0.15f;
        [SerializeField] private string prompt = "Press Button";
        [SerializeField] private UnityEvent OnClick;

        [SerializeField] private AudioSource audioSource;

        public string InteractionPrompt => prompt;

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

            audioSource.Play();

            this.StartTimer(() => { active = true; }, cooldown);
        }
    }
}