using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private NavMeshAgent m_navMeshAgent;
	[SerializeField] private Animator[] m_animatorArray;

	public static Action OnStartMovement;
	public static Action OnDestinationReached;

	private Coroutine m_moveCoroutine;
	private bool m_isMoving;
	public bool IsMoving
	{
		get => m_isMoving;
		set
		{
			m_isMoving = value;

			foreach (Animator m_animator in m_animatorArray)
				m_animator.SetBool("IsMoving", m_isMoving);
		}
	}

	private void Awake ()
	{
		m_oldPos = transform.position;
	}

	private void Update ()
	{
		if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused && !InputManager.grabLocked && InputManager.pointerIn && Input.GetMouseButton(1))
			MoveOnClick();
	}

	private void MoveOnClick ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		bool hasHit = Physics.Raycast(ray, out hit);

		if (hasHit)
		{
			if (hit.collider.CompareTag("Ground"))
			{
				if (m_moveCoroutine != null)
					StopCoroutine(m_moveCoroutine);

				m_moveCoroutine = StartCoroutine(MoveCR(hit.point));
			}
		}
	}

	private void LateUpdate ()
	{
		if (IsMoving && transform.position != m_oldPos)
		{
			Vector3 movement = (transform.position - m_oldPos) * 2f;

			foreach (Animator m_animator in m_animatorArray)
			{
				m_animator.SetFloat("FacingX", Mathf.Clamp(movement.x, -1f, 1f));
				m_animator.SetFloat("FacingZ", Mathf.Clamp(movement.z, -1f, 1f));
			}
			m_oldPos = transform.position;
		}
	}


	private Vector3 m_oldPos;
	IEnumerator MoveCR ( Vector3 destination, Action callback = null )
	{
		OnStartMovement?.Invoke();
		IsMoving = true;
		m_navMeshAgent.destination = destination;
		m_oldPos = transform.position;

		yield return null;

		while (m_navMeshAgent.remainingDistance >= .1f)
		{
			yield return null;
		}

		IsMoving = false;
		OnDestinationReached?.Invoke();
		callback?.Invoke();
	}

	public void MoveToPosition ( Vector3 position, Action callback )
	{
		if (m_moveCoroutine != null)
			StopCoroutine(m_moveCoroutine);

		m_moveCoroutine = StartCoroutine(MoveCR(position, callback));
	}

	public void RotateTowards ( Vector3 position )
	{
		float sign = position.x - transform.position.x;

		if (sign < 0)
		{
			foreach (Animator m_animator in m_animatorArray)
			{
				m_animator.SetFloat("FacingX", -1f);
				m_animator.SetFloat("FacingZ", 0f);
			}
		}
		else if (sign > 0)
		{
			foreach (Animator m_animator in m_animatorArray)
			{
				m_animator.SetFloat("FacingX", 1f);
				m_animator.SetFloat("FacingZ", 0f);
			}
		}
	}
}
