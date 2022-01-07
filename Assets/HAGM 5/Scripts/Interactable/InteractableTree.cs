using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InteractableTree : InteractableGameElement
{
	[Title("References")]
	[SerializeField] private SpriteRenderer[] m_spriteRendererList;
	private SpriteRenderer m_spriteRenderer => m_spriteRendererList[0];
	[SerializeField] private Sprite[] m_spriteArray;
	[SerializeField] private Transform m_transformToTween;

	[Title("Ghost Settings")]
	[SerializeField] private float m_ghostBlendValue = .8f;
	[SerializeField] private float m_noGhostBlendValue = 0f;

	[Title("Tween Settings")]
	[SerializeField] private float m_tweenBaseScale = 1f;
	[SerializeField] private float m_tweenLoopFinalScale = 1.1f;
	[SerializeField] private float m_tweenLoopTime = 0.5f;
	[SerializeField] private float m_tweenToBaseScaleTime = 0.2f;
	[SerializeField] private float m_tweenFinalScale = 0f;
	[SerializeField] private float m_tweenTime = 0.3f;
	[SerializeField] private Ease m_tweenEase = Ease.InBack;

	[Title("Collect Settings")]
	[SerializeField] private float m_chopTime = 1f;
	[SerializeField] private ulong m_woodAmount = 5;
	[SerializeField] private ulong m_dyeAmount = 1;

	private Tweener m_transformTween;

	private void Awake ()
	{
		Sprite randomSprite = m_spriteArray[Random.Range(0, m_spriteArray.Length - 1)];
		bool randomFlip = Random.Range(0, 2) == 0;

		foreach (SpriteRenderer spriteRenderer in m_spriteRendererList)
		{
			spriteRenderer.sprite = randomSprite;
			spriteRenderer.flipX = randomFlip;
		}
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy();
		PlayerMovement.OnStartMovement -= CancelInteraction;
		m_transformTween?.Kill();
	}

	protected override void CancelInteraction ()
	{
		base.CancelInteraction();
		PlayerMovement.OnStartMovement -= CancelInteraction;
		m_transformTween?.Kill();
		m_transformTween = m_transformToTween.DOScale(m_tweenBaseScale, m_tweenToBaseScaleTime);
	}

	protected override IEnumerator InteractCR ()
	{
		PlayerMovement.OnStartMovement += CancelInteraction;
		m_transformTween = m_transformToTween.DOScale(m_tweenLoopFinalScale, m_tweenLoopTime).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
		GameManager.playerController.RotateToPosition(transform.position);

		AudioManager.PlaySfx("TreeChop", Random.Range(.9f, 1.1f));
		yield return new WaitForSeconds(m_chopTime / 2f);
		AudioManager.PlaySfx("TreeChop", Random.Range(.9f, 1.1f));
		yield return new WaitForSeconds(m_chopTime / 2f);

		PlayerMovement.OnStartMovement -= CancelInteraction;
		m_interactable = false;
		CurrencyManager.AddCurrency(Currency.Type.Wood, m_woodAmount);
		CurrencyManager.AddCurrency(Currency.Type.Dye, m_dyeAmount);
		// AudioManager.PlaySfx("TreeFall");

		m_transformTween?.Kill();
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
