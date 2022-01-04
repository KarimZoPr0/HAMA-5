using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour {
   public static Transform[] points;

   public float wayPointRadius = 0.3f;
   private void Awake() {
      points = new Transform[transform.childCount];
      for (var i = 0; i < points.Length; i++) {
         points[i] = transform.GetChild(i);
      }
   }

   private void OnDrawGizmos() {
      for (var i = 0; i < transform.childCount; i++) {
         Gizmos.DrawSphere(transform.GetChild(i).position, wayPointRadius);
      }
     
   }
}
