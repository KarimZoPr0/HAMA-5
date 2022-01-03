using Sirenix.OdinInspector;
using System.Collections;
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
			if (grabLocked && value != CursorType.Grab)
				return;

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

	public static bool grabLocked => Instance.m_grabLocked;
	private bool m_grabLocked = false;

	public static bool canInteract => Instance.m_canInteract;
	private bool m_canInteract = true;

	public static bool canMove => Instance.m_canMove;
	private bool m_canMove = true;

	public static bool pointerIn => Instance.m_pointerIn;
	private bool m_pointerIn = false;

	private WaitForSeconds m_waitAfterGrabBeforeInput = new WaitForSeconds(0.15f);

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

	public static void LockGrab ()
	{
		if (m_releaseGrabCR != null)
			Instance.StopCoroutine(m_releaseGrabCR);

		Instance.m_grabLocked = true;
		Instance.m_canMove = false;
		Instance.m_canInteract = false;

		CursorStatus = CursorType.Grab;
	}

	private static Coroutine m_releaseGrabCR;

	public static void ReleaseGrab ()
	{
		Instance.m_grabLocked = false;

		CursorStatus = CursorType.Cursor;

		m_releaseGrabCR = Instance.StartCoroutine(Instance.ReleaseGrabCR());
	}

	IEnumerator ReleaseGrabCR ()
	{
		yield return m_waitAfterGrabBeforeInput;
		Instance.m_canMove = true;
		Instance.m_canInteract = true;
	}

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