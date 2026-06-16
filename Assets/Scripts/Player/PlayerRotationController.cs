using PlasticGui.WorkspaceWindow.Locks;
using PlasticGui.WorkspaceWindow.QueryViews;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Player
{
	public class PlayerRotationController : MonoBehaviour
	{
        [SerializeField] private Camera viewCamera;
        [SerializeField] private float mouseSensitivity = 75;
        [SerializeField] private float pitchMin = -65;
        [SerializeField] private float pitchMax = 65;
        [Space]
        [SerializeField] private float defaultFOV = 60;
        [SerializeField] private float zoomFOV = 40;
        [SerializeField] private float zoomLerpRate = 0.1f;

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
            viewCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

            bool zoomed = Input.GetMouseButton(1);
            float targetFOV = zoomed ? zoomFOV : defaultFOV;

            if (!Mathf.Approximately(viewCamera.fieldOfView, targetFOV))
            {
                viewCamera.fieldOfView = Mathf.Lerp(viewCamera.fieldOfView, targetFOV, Time.deltaTime / zoomLerpRate);
            }
        }
    }
}