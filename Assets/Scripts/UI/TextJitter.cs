using TMPro;
using UnityEngine;

namespace Gorpozon.WarehouseSim.UI
{
    // Source: https://www.youtube.com/watch?v=FgWVW2PL1bQ

    [RequireComponent(typeof(TMP_Text))]
	public class TextJitter: MonoBehaviour
	{
		[SerializeField] private bool useGradient;
        [SerializeField] private Gradient colorGradient;
        [SerializeField] private Vector2 speed = new Vector2(3f, 2.5f);

        private TMP_Text textMesh;
        private Mesh mesh;
        private Vector3[] vertices;

        void Start()
		{
			textMesh = GetComponent<TMP_Text>();
        }
	
		void Update()
		{
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            Color[] colors = mesh.colors;

            for (int i = 0; i < textMesh.textInfo.characterCount; i++)
            {
                Vector3 offset = Jitter(Time.unscaledTime + i);

                var c = textMesh.textInfo.characterInfo[i];

                int index = c.vertexIndex;

                if (useGradient)
                {
                    colors[index] = colorGradient.Evaluate(Mathf.Repeat(Time.unscaledTime + vertices[index].x * 0.001f, 1f));
                    colors[index + 1] = colorGradient.Evaluate(Mathf.Repeat(Time.unscaledTime + vertices[index + 1].x * 0.001f, 1f));
                    colors[index + 2] = colorGradient.Evaluate(Mathf.Repeat(Time.unscaledTime + vertices[index + 2].x * 0.001f, 1f));
                    colors[index + 3] = colorGradient.Evaluate(Mathf.Repeat(Time.unscaledTime + vertices[index + 3].x * 0.001f, 1f));
                }

                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }

            mesh.vertices = vertices;
            mesh.colors = colors;
            textMesh.canvasRenderer.SetMesh(mesh);
        }

        public void SetSpeed(float value)
        {
            speed = Vector2.one * value;
        }

        public void SetSpeed(Vector2 value)
        {
            speed = value;
        }

        private Vector2 Jitter(float time)
        {
            return new Vector2(Mathf.Sin(time * speed.x), Mathf.Cos(time * speed.y));
        }
    }
}