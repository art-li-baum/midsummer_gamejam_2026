using UnityEngine;

namespace Gorpozon.WarehouseSim.matrerialeffects
{
    public class CrackOnCollison : MonoBehaviour
    {
        public float crackIntestisty = 0.2f; 
        public float treshhold = 2f; 
        
        private Material material;
        private float crackvalue;

        void Start()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                material = meshRenderer.material;
            }
        }
    
        void Update()
        {
            if (material != null)
            {
                crackvalue = material.GetFloat("_crackvalue");
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > treshhold)
            {
                if (crackvalue < 1f)
                {
                    crackvalue = crackvalue + crackIntestisty;
					if(crackvalue> 1)
					{
						crackvalue = 1f;
					}
					material.SetFloat("_crackvalue", crackvalue);
                }
            }
        }
    }
}