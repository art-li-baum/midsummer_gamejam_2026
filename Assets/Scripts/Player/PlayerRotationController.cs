using Gorpozon.WarehouseSim.Services;
using SBG.ServiceLocating;
using UnityEngine;

namespace Gorpozon.WarehouseSim.Player
{
	public class PlayerRotationController : MonoBehaviour
	{
        [SerializeField] private Camera viewCamera;
        [SerializeField] private float pitchMin = -65;
        [SerializeField] private float pitchMax = 65;
        [Space]
        [SerializeField] private float defaultFOV = 60;
        [SerializeField] private float zoomFOV = 40;
        [SerializeField] private float zoomLerpRate = 0.1f;

        private float cameraPitch = 0f;

        private PlayerService playerService;

        private void Start()
        {
            ServiceLocator.TryGet(out playerService);
        }

        private void Update()
        {
            if (playerService.Frozen) return;

            Vector2 input = Input.mousePositionDelta * playerService.MouseSensitivity * Time.deltaTime;

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