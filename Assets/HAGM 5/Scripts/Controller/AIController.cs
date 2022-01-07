using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Modules.UnityMathematics.Editor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum BehaviourState { none, wander, patrol, chase, attack }

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
	public BehaviourState initialState;
	public UnitHealth target;
	public Chase chase;
	public float chaseDistance;

	[Title("Wander Settings")]
	public Bounds boundsBox;

	[Title("Patrol Settings")]
	private List<Transform> patrolPoints;

	[Title("Attack Settings")]
	public DamageDealer damageDealer;
	public float timeBetweenAttacks = 1f;
	[HideIf("canAttackFortress", false)]
	public float slowDown;
	public bool canAttackFortress = false;

	private float timeSinceLastAttack = 0f;

	public bool randomSequence = false;


	private NavMeshAgent agent;
	private Vector3 targetPos;
	private BehaviourState currentState = BehaviourState.none;

	private PatrolPath _patrolPath;

	private void Awake ()
	{
		_patrolPath = FindObjectOfType<PatrolPath>();
		patrolPoints = _patrolPath.patrolPoints;
		agent = GetComponent<NavMeshAgent>();
		m_oldPos = transform.position;
	}

	private void Start ()
	{
		if (canAttackFortress)
		{
			UnitHealth fortress = GameObject.FindGameObjectWithTag("fortress").GetComponent<UnitHealth>();
			target = fortress;
		}
		else
		{
			chaseDistance = chase.chaseDistance;
		}
		SetState(initialState);
	}

	private void SetState ( BehaviourState state )
	{
		if (currentState != state)
		{
			currentState = state;
			if (currentState == BehaviourState.wander)
			{
				FindWanderTarget();
			}
			else if (currentState == BehaviourState.patrol)
			{
				GoToNextPatrolPoint(randomSequence);
			}
		}
	}

	private void Update ()
	{

		if (chase != null && chase.target != null && chase.target)
		{
			target = chase.target.GetComponent<UnitHealth>();
		}

		if (target != null)
		{

			float targetDistance = Vector3.Distance(transform.position, target.transform.position);
			bool canAttack = targetDistance < chaseDistance / 2;

			if (targetDistance < chaseDistance && target.isAlive)
			{
				if (currentState != BehaviourState.chase)
				{
					SetState(BehaviourState.chase);
				}
				else
				{
					if (!canAttack)
					{
						targetPos = target.transform.position;
						agent.SetDestination(targetPos);
					}
					if (agent.isStopped || canAttack)
					{

						if (timeSinceLastAttack <= 0)
						{
							Attack();
							timeSinceLastAttack = 1f / timeBetweenAttacks;
						}
						timeSinceLastAttack -= Time.deltaTime;
					}
				}
			}
			else
			{
				SetState(initialState);
			}
		}

		float distance = Vector3.Distance(targetPos, transform.position);
		if (distance <= agent.stoppingDistance)
		{
			agent.isStopped = true;
			if (currentState == BehaviourState.wander)
			{
				FindWanderTarget();
			}
			else if (currentState == BehaviourState.patrol)
			{
				GoToNextPatrolPoint(randomSequence);
			}
		}
		else if (agent.isStopped)
		{
			agent.isStopped = false;
		}

	}

	[SerializeField] private Animator[] m_animatorArray;
	private Vector3 m_oldPos;
	private void LateUpdate ()
	{
		if (!agent.isStopped && transform.position != m_oldPos)
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

	private void Attack ()
	{
		target.TakeDamage(damageDealer);
		if (!canAttackFortress)
		{
			target.GetComponent<NavMeshAgent>().speed -= slowDown;
		}
	}

	private void FindWanderTarget ()
	{
		targetPos = GetRandomPoint();
		agent.SetDestination(targetPos);
		agent.isStopped = false;
	}

	Vector3 GetRandomPoint ()
	{
		float randomX = Random.Range(-boundsBox.extents.x + agent.radius + boundsBox.center.x, boundsBox.extents.x - agent.radius);
		float randomZ = Random.Range(-boundsBox.extents.z + agent.radius + boundsBox.center.z, boundsBox.extents.z - agent.radius);
		return new Vector3(randomX, transform.position.y, randomZ);
	}

	private void GoToNextPatrolPoint ( bool random = false )
	{
		if (random == false)
		{
			targetPos = GetPatrolPoint();
		}
		else
		{
			targetPos = GetPatrolPoint(true);
		}

		agent.SetDestination(targetPos);
		agent.isStopped = false;
	}

	private Vector3 GetPatrolPoint ( bool random = false )
	{
		var patrolPoints = this.patrolPoints;
		if (random == false)
		{
			if (targetPos == Vector3.zero)
			{
				return patrolPoints[0].position;
			}
			else
			{
				for (var i = 0; i < patrolPoints.Count; i++)
				{
					if (patrolPoints[i].position == targetPos)
					{
						if (i + 1 >= patrolPoints.Count)
						{
							return patrolPoints[0].position;
						}
						else
						{
							return patrolPoints[i + 1].position;
						}
					}

				}
				return GetClosestPatrolPoint();
			}
		}
		else
		{
			return patrolPoints[Random.Range(0, patrolPoints.Count)].position;
		}
	}

	private Vector3 GetClosestPatrolPoint ()
	{
		Transform closest = null;
		foreach (var patrolPoint in patrolPoints)
		{
			if (closest == null)
			{
				closest = patrolPoint;
			}
			else if (Vector3.Distance(transform.position, patrolPoint.position) <
					 Vector3.Distance(transform.position, closest.position))
			{
				closest = patrolPoint;
			}
		}

		return closest.position;
	}
	private void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(boundsBox.center, boundsBox.size);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(targetPos, 0.2f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, chaseDistance);
	}

}
