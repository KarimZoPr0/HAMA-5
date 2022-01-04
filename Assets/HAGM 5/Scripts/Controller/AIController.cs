using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum BehaviourState {none, wander, patrol, chase, attack}

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour {
   public BehaviourState initialState;
   public Transform      target;
   public float          chaseDistance;
   public float          waypointDwellTime;
   
   [Title("Wander Settings")]
   public Bounds boundsBox;

   [Title("Patrol Settings")]
   
   public Transform[] patrolPoints;
   public bool randomSequence = false;

   [Title("Attack Settings")]
   public float weaponRange;
   
   

   private PlayerMovement mover;
   private NavMeshAgent   agent;
   private Vector3        targetPos;
   private BehaviourState currentState = BehaviourState.none;

   private void Awake() {
      agent = GetComponent<NavMeshAgent>();
      mover = GetComponent<PlayerMovement>();
   }

   private void SetState(BehaviourState state) {
      if (currentState != state) {
         currentState = state;
         if (currentState == BehaviourState.wander) {
            Debug.Log("Wander State started");
            StartCoroutine(FindWanderTarget());
         }
         else if (currentState == BehaviourState.patrol) {
            StartCoroutine(GoToNextPatrolPoint(randomSequence));
         }
      }
   }

   private void Update() {
      if (target != null) {
         bool inWeaponRange = Vector3.Distance(transform.position, target.position) < weaponRange;
         if (inWeaponRange) {
            if (currentState != BehaviourState.attack) {
               SetState(BehaviourState.attack);
               Debug.Log("ATTACK STATE"); 
            }
            else {
               targetPos = target.position;
               // Do Some Attack Stuff
            }
         }
         float targetDistance = Vector3.Distance(transform.position, target.position);
         if (targetDistance < chaseDistance) {
            if (currentState != BehaviourState.chase) {
               SetState(BehaviourState.chase);
               Debug.Log("CHASE STATE"); 
            }
            else {
               targetPos = target.position;
               agent.SetDestination(targetPos);
            }
         }
         else {
            SetState(initialState);
         }
      }
      
      float distance = Vector3.Distance(targetPos, transform.position);
      if (distance <= agent.stoppingDistance) {
         agent.isStopped = true;
         if (currentState == BehaviourState.wander) {
            StartCoroutine(FindWanderTarget());
         }
         else if (currentState == BehaviourState.patrol) {
            StartCoroutine(GoToNextPatrolPoint(randomSequence));
         }
      }
      else if (agent.isStopped) {
         agent.isStopped = false;
      }
   }
   

   private IEnumerator FindWanderTarget() {
      targetPos = GetRandomPoint();
      yield return new WaitForSeconds(waypointDwellTime);
      agent.SetDestination(targetPos);
      agent.isStopped = false;
   }

   Vector3 GetRandomPoint() {
      float randomX = Random.Range(-boundsBox.extents.x + agent.radius, boundsBox.extents.x - agent.radius);
      float randomZ = Random.Range(-boundsBox.extents.z + agent.radius, boundsBox.extents.z - agent.radius);
      return new Vector3(randomX, transform.position.y, randomZ);
   }

   private IEnumerator GoToNextPatrolPoint(bool random = false) {
      if (random == false) {
         targetPos = GetPatrolPoint();
      }
      else {
         targetPos = GetPatrolPoint(true);
      }
      yield return new WaitForSeconds(waypointDwellTime);
      agent.SetDestination(targetPos);
      agent.isStopped = false;
   }

   private Vector3 GetPatrolPoint(bool random = false) {
      if (random == false) {
         if (targetPos == Vector3.zero) {
            return patrolPoints[0].position;
         }
         else {
            for (var i = 0; i < patrolPoints.Length; i++) {
               if (patrolPoints[i].position == targetPos) {
                  if (i + 1 >= patrolPoints.Length) {
                     return patrolPoints[0].position;
                  }
                  else {
                     return patrolPoints[i + 1].position;
                  }
               }
               
            }
            return GetClosestPatrolPoint();
         }
      }
      else {
         return patrolPoints[Random.Range(0, patrolPoints.Length)].position;
      }
   }

   private Vector3 GetClosestPatrolPoint() {
      Transform closest = null;
      foreach (var patrolPoint in patrolPoints) {
         if (closest == null) {
            closest = patrolPoint;
         }
         else if (Vector3.Distance(transform.position, patrolPoint.position) <
                  Vector3.Distance(transform.position, closest.position)) {
            closest = patrolPoint;
         }
      }

      return closest.position;
   }
   private void OnDrawGizmos() {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(boundsBox.center, boundsBox.size);
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(targetPos, 0.2f);
      Gizmos.color = Color.grey;
      Gizmos.DrawWireSphere(transform.position, chaseDistance);
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, weaponRange);
      
   }
   
}
