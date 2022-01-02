using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>, IPointerEnterHandler, IPointerExitHandler
{
	[Title("Cursor")]
	[SerializeField] private Texture2D m_handBaseTexture;
	[SerializeField] private Texture2D m_handInteractTexture;
	[SerializeField] private Texture2D m_handGrabTexture;

	private CursorType m_cursorState;
	public static CursorType CursorStatus
	{
		get => Instance.m_cursorState;
		set
		{
			Instance.m_cursorState = value;

			Cursor.visible = CursorStatus != CursorType.None;

			switch (CursorStatus)
			{
				case CursorType.Cursor:
					Cursor.SetCursor(Instance.m_handBaseTexture, m_hotSpot, m_cursorMode);
					break;
				case CursorType.Interact:
					Cursor.SetCursor(Instance.m_handInteractTexture, m_hotSpot, m_cursorMode);
					break;
				case CursorType.Grab:
					Cursor.SetCursor(Instance.m_handGrabTexture, m_hotSpot, m_cursorMode);
					break;
			}
		}
	}

	public static void SetCursor ( CursorType cursorType )
	{
		CursorStatus = cursorType;
	}

	public enum CursorType
	{
		None,
		Cursor,
		Interact,
		Grab,
	}

	private static CursorMode m_cursorMode = CursorMode.ForceSoftware;
	private static Vector2 m_hotSpot = new Vector2(4f, 0f);

	public void Start ()
	{
		CursorStatus = CursorType.Cursor;
	}

	public void OnPointerEnter ( PointerEventData eventData )
	{
		m_pointerIn = true;
	}

	public void OnPointerExit ( PointerEventData eventData )
	{
		m_pointerIn = false;
	}

	private bool m_pointerIn;

	private void Update ()
	{
		if (m_pointerIn)
		{
			float x = Input.mousePosition.x;
			float y = Input.mousePosition.y;

			if (x != 0 || y != 0)
				CameraManager.Instance.PanScreen(x, y);
		}
	}
}