using SBG.Pocketwatch;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Objects
{
	[RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
	public class Package : MonoBehaviour
	{
        [SerializeField] private Vector3 lidCheckOffset;
        [SerializeField] private Vector3 lidCheckSize;
        [SerializeField] private LayerMask lidCheckMask;
        [SerializeField] private float animationDelay = 0.6f;

        private List<GrabbableObject> heldObjects = new();

        private Animator anim;
        private TimerData callbackTimer;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var grabbable = other.GetComponent<GrabbableObject>();
            if (grabbable != null) heldObjects.Add(grabbable);
        }

        private void OnTriggerExit(Collider other)
        {
            var grabbable = other.GetComponent<GrabbableObject>();
            if (grabbable != null) heldObjects.Remove(grabbable);
        }

        public void TryCloseLid(Action onSuccess, Action onFail)
        {
            if (callbackTimer != null) return;

            var hits = Physics.OverlapBox(transform.position + lidCheckOffset, lidCheckSize*0.5f, transform.rotation, lidCheckMask);

            if (hits != null && hits.Length > 0)
            {
                anim.SetTrigger("FailedClose");
                if (onFail != null) callbackTimer = this.StartTimer(onFail, animationDelay);
            }
            else
            {
                foreach (var item in heldObjects)
                {
                    item.FreezeForShipping(transform);
                }

                anim.SetTrigger("Close");
                if (onSuccess != null) callbackTimer = this.StartTimer(onSuccess, animationDelay);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawCube(lidCheckOffset, lidCheckSize);
        }
    }
}