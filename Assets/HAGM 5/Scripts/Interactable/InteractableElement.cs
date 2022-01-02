using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter ( PointerEventData eventData )
	{
		InputManager.SetCursor(InputManager.CursorType.Interact);
	}

	public void OnPointerExit ( PointerEventData eventData )
	{
		InputManager.SetCursor(InputManager.CursorType.Cursor);
	}
}
