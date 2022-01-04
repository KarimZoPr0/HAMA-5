using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class UnitHealth : MonoBehaviour {
     private float Min;
     
	public float  HP;
	public bool           ResetHP;
	public FloatReference StartingHP;
	public UnityEvent     DamageEvent;
	public UnityEvent     DeathEvent;
	public Image Image;
	
	private void Start() {
		if (ResetHP)
			HP =StartingHP;
	}

	public void OnTriggerEnter(Collider other) {
		DamageDealer damage = other.GetComponent<DamageDealer>();
		if (damage != null) {
			HP -= damage.damageAmount;
			DamageEvent.Invoke();
		}

		if (HP <= 0.0f) {
			DeathEvent.Invoke();
		}
		
		SetFillAmount();
	}

	private void SetFillAmount() {
		Image.fillAmount = Mathf.Clamp01(
			Mathf.InverseLerp(Min, StartingHP, HP));
	}
}