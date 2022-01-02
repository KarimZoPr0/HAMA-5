using System;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour {
	private void Update() {
		if (Input.GetMouseButtonDown(1)) {
			SetDestination();
		}
	}

	private void SetDestination() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;

		bool hasHit = Physics.Raycast(ray, out hit);
		if (hasHit) {
			GetComponent<NavMeshAgent>().destination = hit.point;
		}
	}
}
