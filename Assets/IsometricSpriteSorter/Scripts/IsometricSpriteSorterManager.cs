using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode,
DefaultExecutionOrder(-2)]
public class IsometricSpriteSorterManager : MonoBehaviour
{
	[SerializeField] Camera m_camera;
	[SerializeField] private int m_sortingStep = 50;

	// Update is called once per frame
	void LateUpdate ()
	{
		IsometricSpriteSorter.sortingStep = m_sortingStep;
		IsometricSpriteSorter.currentCameraY = m_camera.transform.position.y;
	}
}
