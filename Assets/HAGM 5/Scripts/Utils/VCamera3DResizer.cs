using Cinemachine;
using UnityEngine;

namespace Pinpin
{
	[ExecuteInEditMode]
	public class VCamera3DResizer : MonoBehaviour
	{

		public enum ResizeMode
		{
			None,
			Zoom
		}

		[SerializeField] private float targetFieldOfView = 60f;
		[SerializeField] private Vector2 targetAspectRatio = new Vector2(1242, 2208);
		[SerializeField] private ResizeMode largestResizeMode;
		[SerializeField] private ResizeMode narrowResizeMode = ResizeMode.Zoom;
		[SerializeField] private bool inAnimation;
		[SerializeField] private new CinemachineVirtualCamera camera;
		private bool m_fOVChanged;

		private Vector2Int screenSize { get; set; }
		private int ScreenWidth
		{
			get
			{
#if UNITY_EDITOR
				return (int)UnityEditor.Handles.GetMainGameViewSize().x;
#else
				return Screen.width;
#endif
			}
		}
		private int ScreenHeight
		{
			get
			{
#if UNITY_EDITOR
				return (int)UnityEditor.Handles.GetMainGameViewSize().y;
#else
				return Screen.height;
#endif
			}
		}

		private void OnScreenSizeChanged ()
		{
			m_fOVChanged = false;
			if (ScreenHeight == 0)
				return;

			float currentRatio = ScreenWidth / (float)ScreenHeight;
			float targetRatio = targetAspectRatio.x / targetAspectRatio.y;
			screenSize = new Vector2Int(ScreenWidth, ScreenHeight);

			DefaultSize();
			if (Mathf.Approximately(currentRatio, targetRatio))
				return;

			if (currentRatio > targetRatio)
				LargestResize(currentRatio, targetRatio);
			else
				NarrowResize(currentRatio, targetRatio);
		}

		private void NarrowResize ( float currentRatio, float targetRatio )
		{
			switch (narrowResizeMode)
			{
				case ResizeMode.None:
					break;
				case ResizeMode.Zoom:
					float delta = currentRatio / targetRatio;
					camera.m_Lens.FieldOfView = targetFieldOfView / delta;
					break;
			}
		}

		private void LargestResize ( float currentRatio, float targetRatio )
		{
			switch (largestResizeMode)
			{
				case ResizeMode.None:
					break;
				case ResizeMode.Zoom:
					float delta = targetRatio / currentRatio;
					camera.m_Lens.FieldOfView = targetFieldOfView * delta;
					break;
			}
		}

		public void SetFOV ( float fieldOfView )
		{
			targetFieldOfView = fieldOfView;
			m_fOVChanged = true;
		}

		public float GetFOV ()
		{
			return targetFieldOfView;
		}

		private void DefaultSize ()
		{
			camera.m_Lens.FieldOfView = targetFieldOfView;
		}

		private void Reset ()
		{
			if (camera == null)
			{
				camera = GetComponent<CinemachineVirtualCamera>();
				targetFieldOfView = camera.m_Lens.FieldOfView;
			}
		}

		private void OnValidate ()
		{
			OnScreenSizeChanged();
		}

		private void OnDestroy ()
		{
			if (camera != null)
			{
				camera.m_Lens.FieldOfView = targetFieldOfView;
			}
		}

		private void Update ()
		{
			if (screenSize.x != ScreenWidth || screenSize.y != ScreenHeight || inAnimation || m_fOVChanged)
				OnScreenSizeChanged();
		}
	}
}