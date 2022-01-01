using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter ( PointerEventData eventData )
	{
		InputManager.CursorStatus = InputManager.CursorState.Interact;
	}

	public void OnPointerExit ( PointerEventData eventData )
	{
		InputManager.CursorStatus = InputManager.CursorState.Cursor;
	}
}
