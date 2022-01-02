using Sirenix.OdinInspector;
using UnityEngine;

public class InputManager : Singleton<InputManager>
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

	private static CursorMode m_cursorMode = CursorMode.Auto;
	private static Vector2 m_hotSpot = new Vector2(4f, 0f);

	public void Start ()
	{
		CursorStatus = CursorType.Cursor;
	}

	void OnMouseEnter ()
	{
		Cursor.SetCursor(m_handBaseTexture, m_hotSpot, m_cursorMode);
	}

	void OnMouseExit ()
	{
		Cursor.SetCursor(null, Vector2.zero, m_cursorMode);
	}

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			CursorStatus = CursorType.Cursor;
		if (Input.GetKeyDown(KeyCode.DownArrow))
			CursorStatus = CursorType.None;
		if (Input.GetKeyDown(KeyCode.UpArrow))
			CursorStatus = CursorType.Interact;
		if (Input.GetKeyDown(KeyCode.RightArrow))
			CursorStatus = CursorType.Grab;
	}
}