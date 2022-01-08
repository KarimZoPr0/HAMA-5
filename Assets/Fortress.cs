using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : MonoBehaviour {
	public Bounds bounds;

	public float fortressRadius = 4f;

	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, fortressRadius);
	}
}
