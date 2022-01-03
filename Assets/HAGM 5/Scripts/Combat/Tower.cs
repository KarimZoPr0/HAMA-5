using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Tower : MonoBehaviour {
	private Transform target;
	
	[Title("Attributes")]
	public float range = 15f;
	public  float fireRate      = 1f;
	private float _fireCountDown = 0f;

	[Title("Unity Setup Fields")]
	public GameObject projectile;
	public Transform firePoint;
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	
	
	void UpdateTarget () {
		Elemental[] enemies          = GameObject.FindObjectsOfType<Elemental>();
		float        shortestDistance = Mathf.Infinity;
		GameObject   nearestEnemy     = null;
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

		target = nearestEnemy != null && shortestDistance <= range ? nearestEnemy.transform : null;
	}

	private void Update() {
		if (target == null) return;
		Debug.DrawLine(transform.position, target.position, Color.red);
		if (_fireCountDown <= 0f) {
			Shoot();
			_fireCountDown = 1f / fireRate;
		}

		_fireCountDown -= Time.deltaTime;
	}

	private void Shoot() { 
		GameObject bulletGO = Instantiate(projectile, firePoint.position, firePoint.rotation);
		Projectile bullet   = bulletGO.GetComponent<Projectile>();

		if (bullet != null) {
			bullet.Seek(target);
		}
	}
	
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
