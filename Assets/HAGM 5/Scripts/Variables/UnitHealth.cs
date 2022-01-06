using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class UnitHealth : MonoBehaviour
{
	private float Min;

	public bool isAlive;
	public float HP;
	public bool ResetHP;
	public FloatReference StartingHP;
	public UnityEvent DamageEvent;
	public UnityEvent DeathEvent;
	public Image Image;

	public Action onDeath;

	[SerializeField] private SpriteRenderer m_spriteRenderer;
	private Tweener m_hitTweener;

	public void OnHit ()
	{
		m_hitTweener?.Kill();
		m_spriteRenderer.material.SetFloat("_HitEffectBlend", .25f);
		m_spriteRenderer.material.DOFloat(0f, "_HitEffectBlend", .3f);
	}

	public void OnDeath ()
	{
		m_hitTweener?.Kill();
		m_spriteRenderer.material.SetFloat("_HitEffectBlend", 0f);
		m_spriteRenderer.material.SetFloat("_GreyscaleBlend", 1f);
		StartCoroutine(DisableCR());
	}

	private IEnumerator DisableCR()
	{
		onDeath?.Invoke();
		isAlive = false;
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
	}


	private void Start ()
	{
		if (ResetHP)
		{
			HP = StartingHP;
			isAlive = true;
		}	
	}

	private void OnDestroy ()
	{
		m_hitTweener?.Kill();
	}

	public void TakeDamage (DamageDealer damage)
	{
		if (!isAlive)
			return;

		if (damage != null)
		{
			HP -= damage.damageAmount;
			DamageEvent.Invoke();
			OnHit();
		}

		if (HP <= 0.0f)
		{
			DeathEvent.Invoke();
			OnDeath();
		}

		SetFillAmount();
	}

	private void SetFillAmount ()
	{
		Image.fillAmount = Mathf.Clamp01(
			Mathf.InverseLerp(Min, StartingHP, HP));
	}
}