using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Tower : MonoBehaviour {
	[Title("Attributes")]
	public  float fireRate      = 1f;
	private float _fireCountDown = 0f;
	public  bool  canFire = true;

	[Title("Unity Setup Fields")]
	
	public Chase      chase;
	public GameObject projectile;
	public Transform  firePoint;

	private void Update() {
		if (chase.target == null) return;
		if (!canFire) return;
		Debug.DrawLine(transform.position, chase.target.position, Color.red);
		if (_fireCountDown <= 0f) {
			Shoot();
			_fireCountDown = 1f / fireRate;
		}

		_fireCountDown -= Time.deltaTime;
	}

	private void Shoot() {
		GameObject bulletGO = Instantiate(projectile, firePoint.position, firePoint.rotation);
		Projectile bullet   = bulletGO.GetComponent<Projectile>();

		bullet.Seek(chase.target);
	}
}
