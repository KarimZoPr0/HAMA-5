using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTree : InteractableGameElement
{
	[Title("References")]
	[SerializeField] private SpriteRenderer m_spriteRenderer;

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
		CurrencyManager.AddCurrency(Currency.Type.Wood, 5);
		CurrencyManager.AddCurrency(Currency.Type.Dye, 1);
		AudioManager.PlaySfx("Bop");
		m_transformTween = transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
		{
			Destroy(gameObject);
		});
	}

	protected override void OnStartHover ()
	{
		m_spriteRenderer.material.SetFloat("_GhostBlend", 0.8f);
	}

	protected override void OnExitHover ()
	{
		m_spriteRenderer.material.SetFloat("_GhostBlend", 0f);
	}
}
