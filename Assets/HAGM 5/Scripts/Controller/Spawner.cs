using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : Singleton<Spawner> {
	public Transform enemyPrefab;
	
	public Transform spawnPoint;
	public bool      canSpawn = true;

	private PatrolPath _patrolPath;
	public  UnitHealth fortress;

	public int   spawnLimit       = 10;
	public float timeBetweenSpawn = 2f;
	
	public  int   enemiesNum = 0;
	private float countDown  = 2f;
	private int   waveNum = 1;
	
	
	

	private void Update() {
		if (!canSpawn) return;

		if (!fortress.isAlive) {
			canSpawn = false;
		}
		
		if (enemiesNum > spawnLimit) {
			return;
		}
		if (countDown <= 0f) {
			Spawn(enemyPrefab, spawnPoint.position);
			countDown = timeBetweenSpawn;
		}
		countDown -= Time.deltaTime;
	}



	public void Spawn(Transform prefab, Vector3 position) {
		Instantiate(prefab,position, spawnPoint.rotation);
		enemiesNum++;
	}
	
	
}
