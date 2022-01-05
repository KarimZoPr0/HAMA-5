using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour {
   public List<Transform> patrolPoints;

   private void Awake() {
      for (var i = 0; i < transform.childCount; i++) {
         patrolPoints.Add(transform.GetChild(i)); 
      }
   }
}
