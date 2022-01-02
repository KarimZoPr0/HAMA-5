using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Config/CameraConfig", order = 3)]
public class CameraConfig : ScriptableObject
{
	[Title("Pan Settings")]
	public float panScreenPercent = 0.1f;
	public float panSpeed = 8f;

	[Title("Zoom Settings")]
	public float zoomSpeed = 50f;
	public float zoomFovMax = 70f;
	public float zoomFovMin = 20f;
	public float zoomMultiplier = -250f;
}
