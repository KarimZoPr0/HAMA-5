using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Panel : MonoBehaviour
{
	public enum Type
	{
		Main,
		Game,
		Pause,
		End,
	}

	[Title("Panel Infos")]
	[SerializeField] private CanvasGroup m_canvasGroup;
	private Tweener m_canvasGroupTween;
	public Type type;

	void OnDestroy()
	{
		m_canvasGroupTween?.Kill();
	}

	public virtual Panel OpenPanel ( bool _fade )
	{
		gameObject.SetActive(true);

		if (m_canvasGroupTween != null)
			m_canvasGroupTween.Kill();

		if (_fade)
		{
			m_canvasGroup.alpha = 0f;
			m_canvasGroupTween = m_canvasGroup.DOFade(1f, 0.5f);
		}
		else
			m_canvasGroup.alpha = 1f;

		return this;
	}

	public virtual Panel ClosePanel ( bool _fade )
	{
		if (m_canvasGroupTween != null)
			m_canvasGroupTween.Kill();

		if (_fade)
			m_canvasGroupTween = m_canvasGroup.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));
		else
		{
			m_canvasGroup.alpha = 0f;
			gameObject.SetActive(false);
		}

		return this;
	}

	public virtual void Init ()
	{

	}
}
