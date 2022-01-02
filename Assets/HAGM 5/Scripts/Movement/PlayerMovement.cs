using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private NavMeshAgent m_navMeshAgent;
	[SerializeField] private SpriteRenderer m_spriteRenderer;
	public static Action OnStartMovement;
	public static Action OnDestinationReached;

	private Coroutine m_moveCoroutine;
	public bool isMoving = false;

	private void Reset ()
	{
		m_navMeshAgent = GetComponent<NavMeshAgent>();
	}

	private void Update ()
	{
		if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused && Input.GetMouseButtonDown(1))
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

	IEnumerator MoveCR ( Vector3 destination, Action callback = null )
	{
		OnStartMovement?.Invoke();
		isMoving = true;
		m_navMeshAgent.destination = destination;
		yield return null;

		while (m_navMeshAgent.remainingDistance >= .1f)
		{
			float sign = Mathf.Sign(m_navMeshAgent.velocity.x);

			if (sign < 0)
				m_spriteRenderer.flipX = true;
			else if (sign > 0)
				m_spriteRenderer.flipX = false;

			yield return null;
		}	

		isMoving = false;
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
			m_spriteRenderer.flipX = true;
		else if (sign > 0)
			m_spriteRenderer.flipX = false;
	}
}
