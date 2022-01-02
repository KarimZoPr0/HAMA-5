using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour {
    
	public FloatVariable  HP;
	public bool           ResetHP;
	public FloatReference StartingHP;
	public UnityEvent     DamageEvent;
	public UnityEvent     DeathEvent;

	private void Start() {
		if (ResetHP)
			HP.SetValue(StartingHP);
	}

	private void OnTriggerEnter(Collider other) {
		DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();
		if (damage != null) {
			HP.ApplyChange(-damage.damageAmount);
			DamageEvent.Invoke();
		}

		if (HP.Value <= 0.0f) {
			DeathEvent.Invoke();
		}
	}
}