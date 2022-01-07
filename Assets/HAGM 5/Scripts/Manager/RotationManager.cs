using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : Singleton<RotationManager>
{
	[OnValueChanged("RotateEverything"), SerializeField] private float m_xRotation;
	public static float overallXRotation => Instance.m_xRotation;

	private void RotateEverything ()
	{
		Rotator[] rotators = FindObjectsOfType<Rotator>();
		foreach (Rotator rotator in rotators)
			rotator.RotateToXRotation(m_xRotation);
	}
}
