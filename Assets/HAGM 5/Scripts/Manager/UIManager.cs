using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	[Title("Panels")]
	[SerializeField] private List<Panel> m_panelList;

	#region Panel
	private static Dictionary<Panel.Type , Panel> m_panelDictionary;

	public override void Awake ()
	{
		base.Awake();
		m_panelDictionary = m_panelList.ToDictionary(p => p.type);
	}

	public static Panel GetPanel ( Panel.Type _panelType )
	{
		return m_panelDictionary[_panelType];
	}

	public static Panel OpenPanel ( Panel.Type _panelType, bool _fade )
	{
		return m_panelDictionary[_panelType].OpenPanel(_fade);
	}

	public static Panel ClosePanel ( Panel.Type _panelType, bool _fade )
	{
		return m_panelDictionary[_panelType].ClosePanel(_fade);
	}
	#endregion

	[Title("Fade")]
	[SerializeField] private float m_fadeTime;
	[SerializeField] private Image m_fadeImage;

	#region Fade
	public static Action OnFadeInFinishedAction;
	public static Action OnFadeOutFinishedAction;

	public void FadeIn ()
	{
		m_fadeImage.gameObject.SetActive(true);
		m_fadeImage.DOColor(Color.black, m_fadeTime).SetUpdate(true).SetEase(Ease.InOutSine).onComplete += OnFadeInFinished;
	}

	void OnFadeInFinished ()
	{
		OnFadeInFinishedAction?.Invoke();
		OnFadeInFinishedAction = null;
	}

	public void FadeOut ()
	{
		m_fadeImage.DOColor(Color.clear, m_fadeTime).SetUpdate(true).SetEase(Ease.InOutSine).onComplete += OnFadeOutFinished;
	}

	void OnFadeOutFinished ()
	{
		OnFadeOutFinishedAction?.Invoke();
		OnFadeOutFinishedAction = null;
		m_fadeImage.gameObject.SetActive(false);
	}
	#endregion
}
