using DG.Tweening;
using System;
using UnityEngine;

public class TreeEntity : MonoBehaviour
{
	public static Action<TreeEntity> onTreeDestroyed;
	private Tweener m_spawnTween;

	public void SpawnAnimation()
	{
		transform.localScale = Vector3.zero;
		m_spawnTween = transform.DOScale(1f, .4f).SetEase(Ease.OutBack).OnComplete(() => AudioManager.PlaySfx("TreeSpawn"));
	}

	private void OnDestroy ()
	{
		m_spawnTween?.Kill();
		onTreeDestroyed?.Invoke(this);
	}
}
