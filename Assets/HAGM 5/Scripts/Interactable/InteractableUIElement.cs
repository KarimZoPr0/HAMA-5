using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private bool m_pointerIn = false;

	public void OnPointerEnter ( PointerEventData eventData )
	{
		if (!m_pointerIn)
		{
			m_pointerIn = true;
			InputManager.SetCursor(InputManager.CursorType.Interact);
		}
	}

	public void OnPointerExit ( PointerEventData eventData )
	{
		if (m_pointerIn)
		{
			m_pointerIn = false;
			InputManager.SetCursor(InputManager.CursorType.Cursor);
		}		
	}

	private void Update ()
	{
		if (m_pointerIn)
		{
			if (InputManager.CursorStatus != InputManager.CursorType.Interact)
				InputManager.SetCursor(InputManager.CursorType.Interact);
		}
	}
}
