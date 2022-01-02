using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanAndZoomCamera : MonoBehaviour
{
	[Title("Camera Configs")]
	[SerializeField] private CameraConfig m_cameraConfig;

	[Title("References")]
	[SerializeField] private Transform m_cameraTransform;
	[SerializeField] private CinemachineVirtualCamera m_cinemachineVirtualCamera;
	[SerializeField] private Transform m_playerTransform;
	private Vector3 m_playerOffset;

	private void Awake ()
	{
		m_playerOffset = m_playerTransform.position - m_cameraTransform.position;
	}

	private void Reset ()
	{
		m_cameraTransform = transform;
		m_cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
	}

	private void ResetCamera ()
	{
		m_cameraTransform.position = m_playerTransform.position - m_playerOffset;
	}

	private void Update ()
	{
		if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused)
		{
			if (Input.GetKeyDown(KeyCode.Space))
				ResetCamera();

			if (Input.GetAxis("Mouse ScrollWheel") != 0)
				ZoomScreen(Input.GetAxis("Mouse ScrollWheel") * m_cameraConfig.zoomMultiplier);
		}
	}

	private void ZoomScreen (float increment)
	{
		float fov = m_cinemachineVirtualCamera.m_Lens.FieldOfView;
		float targetFov = Mathf.Clamp(fov + increment, m_cameraConfig.zoomFovMin, m_cameraConfig.zoomFovMax);
		m_cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, targetFov, m_cameraConfig.zoomSpeed * Time.deltaTime);
	}

	private Vector2 PanDirection ( float x, float y )
	{
		Vector2 direction = Vector2.zero;

		if (x >= Screen.width * (1f - m_cameraConfig.panScreenPercent) && x <= Screen.width)
			direction.x += 1;
		else if (x <= Screen.width * m_cameraConfig.panScreenPercent && x >= 0)
			direction.x -= 1;

		if (y >= Screen.height * (1f - m_cameraConfig.panScreenPercent) && y <= Screen.height)
			direction.y += 1;
		else if (y <= Screen.height * m_cameraConfig.panScreenPercent && y >= 0)
			direction.y -= 1;

		return direction;
	}

	public void PanScreen ( float x, float y )
	{
		Vector2 direction = PanDirection(x, y);
		Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
		m_cameraTransform.position = Vector3.Lerp(m_cameraTransform.position, m_cameraTransform.position + moveDirection * m_cameraConfig.panSpeed, Time.deltaTime);
	}
}
