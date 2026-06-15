using UnityEngine;

namespace Gorpozon.WarehouseSim.Player
{
	public class PlayerRotationController : MonoBehaviour
	{
        [SerializeField] private Transform viewCamera;
        [SerializeField] private float mouseSensitivity = 75;
        [SerializeField] private float pitchMin = -65;
        [SerializeField] private float pitchMax = 65;

        private float cameraPitch = 0f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Vector2 input = Input.mousePositionDelta * mouseSensitivity * Time.deltaTime;

            cameraPitch -= input.y;
            cameraPitch = Mathf.Clamp(cameraPitch, pitchMin, pitchMax);

            transform.Rotate(Vector3.up * input.x);
            viewCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }
}