using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InteractableGameElement : MonoBehaviour
{
	protected bool m_mouseOver = false;
	protected bool m_interactable = true;
	private Coroutine m_interactionCoroutine;

	protected virtual void OnDestroy ()
	{
		PlayerMovement.OnStartMovement -= CancelInteraction;

		if (m_mouseOver)
		{
			m_mouseOver = false;
			InputManager.SetCursor(InputManager.CursorType.Cursor);
			OnExitHover();
		}
	}

	protected virtual void CancelInteraction ()
	{
		PlayerMovement.OnStartMovement -= CancelInteraction;

		if (m_interactionCoroutine != null)
			StopCoroutine(m_interactionCoroutine);
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

	private void Update ()
	{
		if (m_interactable && m_mouseOver)
		{
			if (Input.GetMouseButtonDown(1))
				GameManager.playerController.MoveToInteract(transform.position - 2f * Vector3.forward + Random.Range(-1, 2) * Vector3.right, Interact);
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

	protected virtual void Interact ()
	{
		PlayerMovement.OnStartMovement += CancelInteraction;
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
