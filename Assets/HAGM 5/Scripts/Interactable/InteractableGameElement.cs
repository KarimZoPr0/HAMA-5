using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InteractableGameElement : MonoBehaviour
{
	protected bool m_mouseOver = false;
	protected bool m_interactable = true;
	protected Coroutine m_interactionCoroutine;

	protected virtual void OnDestroy ()
	{
		if (m_mouseOver)
		{
			m_mouseOver = false;
			InputManager.SetCursor(InputManager.CursorType.Cursor);
			OnExitHover();
		}
	}

	protected virtual void CancelInteraction ()
	{
		if (m_interactionCoroutine != null)
			StopCoroutine(m_interactionCoroutine);
	}

	public void OnMouseOver ()
	{
		if (!InputManager.canInteract)
			return;

		if (!m_mouseOver)
		{
			if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused && m_interactable&& !InputManager.grabLocked)
			{
				m_mouseOver = true;
				InputManager.SetCursor(InputManager.CursorType.Interact);
				OnStartHover();
			}
		}
		else
		{
			if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused && m_interactable && !InputManager.grabLocked)
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

	private void Update ()
	{
		if (!InputManager.canInteract)
			return;

		if (m_interactable && m_mouseOver && InputManager.CursorStatus == InputManager.CursorType.Interact)
		{
			if (Input.GetMouseButtonDown(1) && !InputManager.grabLocked)
			{
				TryToInteract();
			}	
		}
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

	protected virtual void TryToInteract()
	{
		GameManager.playerController.MoveToInteract(transform.position - 2f * Vector3.forward + Random.Range(-.7f, .7f) * Vector3.right, Interact);
	}

	protected virtual void Interact ()
	{
		m_interactionCoroutine = StartCoroutine(InteractCR());
	}

	protected abstract IEnumerator InteractCR ();

	protected virtual void OnStartHover ()
	{

	}

	protected virtual void OnExitHover ()
	{

	}
}
