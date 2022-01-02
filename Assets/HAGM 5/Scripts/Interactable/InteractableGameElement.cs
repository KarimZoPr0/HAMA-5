using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableGameElement : MonoBehaviour
{
	protected bool m_mouseOver = false;
	protected bool m_interactable = true;

	private void OnDestroy ()
	{
		if (m_mouseOver)
		{
			m_mouseOver = false;
			InputManager.SetCursor(InputManager.CursorType.Cursor);
			OnExitHover();
		}
	}

	public void OnMouseOver ()
	{
		if (!m_mouseOver)
		{
			if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused && m_interactable)
			{
				m_mouseOver = true;
				InputManager.SetCursor(InputManager.CursorType.Interact);
				OnStartHover();
			}
		}
		else
		{
			if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused && m_interactable)
			{
				if (InputManager.CursorStatus != InputManager.CursorType.Interact)
					InputManager.SetCursor(InputManager.CursorType.Interact);
			}
			else if (GameManager.Instance.GameState != GameState.InGame || !m_interactable || GameManager.Instance.isPaused)
			{
				OnMouseExit();
			}
		}
	}

	public void OnMouseDown ()
	{
		if (m_interactable && m_mouseOver)
			Interact();
	}

	public void OnMouseExit ()
	{
		if (m_mouseOver)
		{
			m_mouseOver = false;
			InputManager.SetCursor(InputManager.CursorType.Cursor);
			OnExitHover();
		}
	}

	protected virtual void Interact ()
	{

	}

	protected virtual void OnStartHover ()
	{
		//GetComponent<SpriteRenderer>().material.SetFloat("_GhostBlend", 0.8f);
	}

	protected virtual void OnExitHover ()
	{
		//GetComponent<SpriteRenderer>().material.SetFloat("_GhostBlend", 0f);
	}
}
