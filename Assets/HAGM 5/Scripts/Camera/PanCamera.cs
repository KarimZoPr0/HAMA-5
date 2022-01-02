using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
	[Title("Settings")]
	[SerializeField] private float m_panScreenPercent = .05f;
	[SerializeField] private float m_panSpeed = 5;

	[Title("References")]
	[SerializeField] private Transform m_cameraTransform;
	[SerializeField] private Transform m_playerTransform;
	private Vector3 m_playerOffset;

	private void Awake ()
	{
		m_playerOffset = m_playerTransform.position - m_cameraTransform.position;
	}

	private void Reset ()
	{
		m_cameraTransform = transform;
	}

	private void ResetCamera ()
	{
		m_cameraTransform.position = m_playerTransform.position - m_playerOffset;
	}

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			ResetCamera();
	}

	private Vector2 PanDirection ( float x, float y )
	{
		Vector2 direction = Vector2.zero;

		if (x >= Screen.width * (1f - m_panScreenPercent) && x <= Screen.width)
			direction.x += 1;
		else if (x <= Screen.width * m_panScreenPercent && x >= 0)
			direction.x -= 1;

		if (y >= Screen.height * (1f - m_panScreenPercent) && y <= Screen.height)
			direction.y += 1;
		else if (y <= Screen.height * m_panScreenPercent && y >= 0)
			direction.y -= 1;

		return direction;
	}

	public void PanScreen ( float x, float y )
	{
		Vector2 direction = PanDirection(x, y);
		Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
		m_cameraTransform.position = Vector3.Lerp(m_cameraTransform.position, m_cameraTransform.position + moveDirection * m_panSpeed, Time.deltaTime);
	}
}
