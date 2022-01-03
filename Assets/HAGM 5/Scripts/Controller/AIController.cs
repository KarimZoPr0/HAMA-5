using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour {
   private NavMeshAgent agent;

   public float speed;
   public float walkRadius;

   private void Start() {
      agent = GetComponent<NavMeshAgent>();
      if (agent != null) {
         agent.speed = speed;
         agent.SetDestination(RandomNavmeshLocaltion());
      }
   }

   private void Update() {
      if (agent != null && agent.remainingDistance <= agent.stoppingDistance) {
         agent.SetDestination(RandomNavmeshLocaltion());
      }
   }

   private Vector3 RandomNavmeshLocaltion() {
     Vector3 finalPosition  = Vector3.zero;
     Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
     randomPosition += transform.position;
     NavMeshHit hit;
     if (NavMesh.SamplePosition(randomPosition, out hit, walkRadius, 1)) {
        finalPosition = hit.position;
     }

     return finalPosition;
   }

   private void OnDrawGizmosSelected() {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(transform.position, walkRadius);
   }
}
