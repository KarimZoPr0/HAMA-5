using Sirenix.OdinInspector;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	[Title("Cursor")]
	[SerializeField] private Texture2D m_handBaseTexture;
	[SerializeField] private Texture2D m_handInteractTexture;
	[SerializeField] private Texture2D m_handGrabTexture;

	private CursorState m_cursorState;
	public static CursorState CursorStatus
	{
		get => Instance.m_cursorState;
		set
		{
			Instance.m_cursorState = value;

			Cursor.visible = CursorStatus != CursorState.None;

			switch (CursorStatus)
			{
				case CursorState.Cursor:
					Cursor.SetCursor(Instance.m_handBaseTexture, m_hotSpot, m_cursorMode);
					break;
				case CursorState.Interact:
					Cursor.SetCursor(Instance.m_handInteractTexture, m_hotSpot, m_cursorMode);
					break;
				case CursorState.Grab:
					Cursor.SetCursor(Instance.m_handGrabTexture, m_hotSpot, m_cursorMode);
					break;
			}
		}
	}

	public enum CursorState
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
		CursorStatus = CursorState.Cursor;
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
			CursorStatus = CursorState.Cursor;
		if (Input.GetKeyDown(KeyCode.DownArrow))
			CursorStatus = CursorState.None;
		if (Input.GetKeyDown(KeyCode.UpArrow))
			CursorStatus = CursorState.Interact;
		if (Input.GetKeyDown(KeyCode.RightArrow))
			CursorStatus = CursorState.Grab;
	}
}