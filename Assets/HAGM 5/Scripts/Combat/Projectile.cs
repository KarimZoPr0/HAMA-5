using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour {
	private Transform    target;

	public float      speed = 70f;
	public void Seek(Transform target) {
		this.target = target;
		
	}

	private void Update() {
		if (target == null) {
			Destroy(gameObject);
			return;
		}

		Vector3 dir               = target.position - transform.position;
		float   distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude < distanceThisFrame) {
			HitTarget();
			return;
		}
		
		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
	}
	

	public void HitTarget() {
		Debug.Log("HIT TARGET");
		Destroy(gameObject, 2f);
	}
}
