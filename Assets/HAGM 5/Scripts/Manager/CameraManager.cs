using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
	[SerializeField] private CinemachineBrain m_cinemachineBrain;
	[SerializeField] private CinemachineVirtualCamera m_activeCamera;
	public static CinemachineBrain cinemachineBrain => Instance.m_cinemachineBrain;

	[Title("Cameras")]
	[SerializeField] private PanCamera m_panCamera;
	[SerializeField] private List<CameraPair> m_cameraList;

	private Dictionary<string, CinemachineVirtualCamera> m_cameraDictionary;

	public override void Awake ()
	{
		base.Awake();

		m_cameraDictionary = new Dictionary<string, CinemachineVirtualCamera>();

		foreach (CameraPair camera in m_cameraList)
			if (!m_cameraDictionary.ContainsKey(camera.name))
				m_cameraDictionary.Add(camera.name, camera.virtualCamera);
	}

	public static void SetCamera ( string cameraName )
	{
		if (Instance.m_cameraDictionary.ContainsKey(cameraName))
		{
			Instance.m_activeCamera.Priority = 0;
			Instance.m_cameraDictionary[cameraName].Priority = 10;
			Instance.m_activeCamera = Instance.m_cameraDictionary[cameraName];
		}
	}

	public static void ReturnToMainCamera ()
	{
		Instance.m_activeCamera.Priority = 0;
		Instance.m_cameraDictionary["MainCamera"].Priority = 10;
		Instance.m_activeCamera = Instance.m_cameraDictionary["MainCamera"];
	}

	public void PanScreen ( float x, float y )
	{
		m_panCamera.PanScreen(x, y);
	}
}

[System.Serializable]
public class CameraPair
{
	public string name;
	public CinemachineVirtualCamera virtualCamera;
}

