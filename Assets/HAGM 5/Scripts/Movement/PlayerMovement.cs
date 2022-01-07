using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
	private const string WALK_FORWARD = "walk_forward";
	private const string WALK_HORIZONTAL= "walk_horizontal";
	private const string WALK_BACKWARD= "walk_backward";
	private const string CHOP = "chop";
	private const string ATTACK = "attack";
	
	
	[SerializeField] private NavMeshAgent m_navMeshAgent;
	[SerializeField] private SpriteRenderer m_spriteRenderer;
	public static Action OnStartMovement;
	public static Action OnDestinationReached;

	

	private Coroutine m_moveCoroutine;
	public bool isMoving = false;

	private string currentState;
	private void Reset ()
	{
		if(m_navMeshAgent )
		m_navMeshAgent = GetComponent<NavMeshAgent>();
	}


	public void ChangeAnimationState(string newState) {
		if (currentState == newState) return;
		
		anim.Play(newState);

		currentState = newState;
	}
	
	public Animator anim;
	public TMP_Text State;
	private void Update () {

		if (GameManager.Instance.GameState == GameState.InGame && !GameManager.Instance.isPaused && !InputManager.grabLocked && InputManager.pointerIn && Input.GetMouseButton(1))
			MoveOnClick();

		if (!isMoving) return;
		HandleAnimations();
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

		HandleAnimations();

		isMoving = false;
		OnDestinationReached?.Invoke();
		callback?.Invoke();
	}

	private void HandleAnimations() {
		var x = m_navMeshAgent.velocity.x;
		var z = m_navMeshAgent.velocity.z;
               		
		bool a = x > z;
		bool b = x > -z;

		switch (a) {
			case true when b:
				State.text             = "right";
				m_spriteRenderer.flipX = false;
				ChangeAnimationState(WALK_HORIZONTAL);
				break;
			case false when !b:
				State.text             = "left";
				m_spriteRenderer.flipX = true;
				ChangeAnimationState(WALK_HORIZONTAL); // left
				break;
			case false when b:
				State.text = "up";
				ChangeAnimationState(WALK_BACKWARD); // up
				break;
			case true when !b:
				State.text = "down";
				ChangeAnimationState(WALK_FORWARD); // down
				break;
		}
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
