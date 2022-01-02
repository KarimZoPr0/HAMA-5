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

	[Title("Panel Fading")]
	[SerializeField] private CanvasGroup m_canvasGroup;
	[SerializeField] private float m_fadeTime = 0.5f;
	private Tweener m_canvasGroupTween;
	public Type type;

	protected virtual void OnDestroy ()
	{
		m_canvasGroupTween?.Kill();
	}

	public virtual Panel OpenPanel ( bool _fade )
	{
		gameObject.SetActive(true);

		m_canvasGroupTween?.Kill();

		if (_fade)
		{
			m_canvasGroup.alpha = 0f;
			m_canvasGroupTween = m_canvasGroup.DOFade(1f, m_fadeTime);
		}
		else
			m_canvasGroup.alpha = 1f;

		return this;
	}

	public virtual Panel ClosePanel ( bool _fade )
	{
		m_canvasGroupTween?.Kill();

		if (_fade)
			m_canvasGroupTween = m_canvasGroup.DOFade(0f, m_fadeTime).OnComplete(() => gameObject.SetActive(false));
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
