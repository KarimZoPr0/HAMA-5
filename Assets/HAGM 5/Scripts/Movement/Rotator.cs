using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	private float m_xRotation = 40f;

	public void RotateToXRotation (float xRotation)
	{
		m_xRotation = xRotation;
		transform.rotation = Quaternion.Euler(new Vector3(m_xRotation, 0, 0));
	}

	private void Start ()
	{
		RotateToXRotation(RotationManager.overallXRotation);
	}
}
