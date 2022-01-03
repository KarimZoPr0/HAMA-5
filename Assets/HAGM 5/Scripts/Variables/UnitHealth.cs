using UnityEngine;
using UnityEngine.Events;

public class UnitHealth : MonoBehaviour {
    
	public FloatReference  HP;
	public bool           ResetHP;
	public FloatReference StartingHP;
	public UnityEvent     DamageEvent;
	public UnityEvent     DeathEvent;

	private void Start() {
		if (ResetHP)
			HP.Variable.SetValue(StartingHP);
	}

	public void TakeDamage(DamageDealer damage) {
		if (damage != null) {
			HP.Variable.ApplyChange(-damage.damageAmount);
			DamageEvent.Invoke();
		}

		if (HP.Value <= 0.0f) {
			DeathEvent.Invoke();
		}
	}
}