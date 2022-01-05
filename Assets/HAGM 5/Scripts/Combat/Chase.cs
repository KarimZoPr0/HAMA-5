using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour {
	public Transform target;
	public float     chaseDistance;
	public Transform RangeSprite;

	void Start () {
		var towerRange  = chaseDistance * 2;
		if (RangeSprite != null) {
			SetSpriteRange(towerRange);
		}
		InvokeRepeating("SetChase", 0f, 0.5f);
	}

	private void SetSpriteRange(float towerRange) {
		RangeSprite.localScale = new Vector3(towerRange, towerRange, towerRange);
	}


	public void SetChase() {
		Elemental[] enemies          = FindObjectsOfType<Elemental>();
		float       shortestDistance = Mathf.Infinity;
		Elemental nearestEnemy     = null;
		foreach (var enemy in enemies)
		{
			if(!GetComponent<Elemental>().element.defeatAbleElements.Contains(enemy.element)) continue;
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy     = enemy;
			}
		}

		target = nearestEnemy != null && shortestDistance <= chaseDistance && nearestEnemy ? nearestEnemy.transform : null;
	}
	
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, chaseDistance);
	}
}
