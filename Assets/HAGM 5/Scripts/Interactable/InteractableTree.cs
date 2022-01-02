using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTree : InteractableGameElement
{
	[Title("References")]
	[SerializeField] private SpriteRenderer m_spriteRenderer;
	[SerializeField] private Transform m_transformToTween;

	[Title("Ghost Settings")]
	[SerializeField] private float m_ghostBlendValue = .8f;
	[SerializeField] private float m_noGhostBlendValue = 0f;

	[Title("Tween Settings")]
	[SerializeField] private float m_tweenFinalScale = 0f;
	[SerializeField] private float m_tweenTime = .3f;
	[SerializeField] private Ease m_tweenEase = Ease.InBack;

	[Title("Collect Settings")]
	[SerializeField] private ulong m_woodAmount = 5;
	[SerializeField] private ulong m_dyeAmount = 1;

	private Tweener m_transformTween;

	private void Reset ()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnDestroy ()
	{
		m_transformTween?.Kill();
	}

	protected override void Interact ()
	{
		m_interactable = false;
		CurrencyManager.AddCurrency(Currency.Type.Wood, m_woodAmount);
		CurrencyManager.AddCurrency(Currency.Type.Dye, m_dyeAmount);
		AudioManager.PlaySfx("Bop");
		m_transformTween = m_transformToTween.DOScale(m_tweenFinalScale, m_tweenTime).SetEase(m_tweenEase).OnComplete(() =>
		{
			Destroy(m_transformToTween.gameObject);
		});
	}

	protected override void OnStartHover ()
	{
		m_spriteRenderer.material.SetFloat("_GhostBlend", m_ghostBlendValue);
	}

	protected override void OnExitHover ()
	{
		m_spriteRenderer.material.SetFloat("_GhostBlend", m_noGhostBlendValue);
	}
}
