using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour {
	public Transform target { get; private set; }
	public float     chaseDistance;

	void Start () {
		InvokeRepeating("SetChase", 0f, 0.5f);
	}

	public void SetChase() {
		Elemental[] enemies          = FindObjectsOfType<Elemental>();
		float       shortestDistance = Mathf.Infinity;
		GameObject  nearestEnemy     = null;
		foreach (var enemy in enemies)
		{
			if(!GetComponent<Elemental>().element.defeatAbleElements.Contains(enemy.element)) continue;
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy     = enemy.gameObject;
			}
		}

		target = nearestEnemy != null && shortestDistance <= chaseDistance ? nearestEnemy.transform : null;
	}
	
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, chaseDistance);
	}
}
