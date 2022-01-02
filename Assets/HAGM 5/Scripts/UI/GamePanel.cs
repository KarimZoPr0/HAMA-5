using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : Panel
{
	[Title("Buttons")]
	[SerializeField] private Button m_pauseBtn;
	[Title("Currencys")]
	[SerializeField] private List<CurrencyUIPair> m_currencyUIList;

	private Dictionary<Currency.Type, CurrencyUI> m_currencyUIDictionary;

	private void Awake ()
	{
		m_currencyUIDictionary = new Dictionary<Currency.Type, CurrencyUI>();

		foreach (CurrencyUIPair currencyUIPair in m_currencyUIList)
		{
			if (!m_currencyUIDictionary.ContainsKey(currencyUIPair.currencyType))
			{
				m_currencyUIDictionary.Add(currencyUIPair.currencyType, currencyUIPair.currencyUI);
				currencyUIPair.currencyUI.Init(currencyUIPair.currencyType);
			}
		}

		CurrencyManager.OnCurrencyChange += RefreshCurrencyAmount;

		m_pauseBtn.onClick.AddListener(OnClickPause);
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy();

		m_pauseBtn.onClick.RemoveListener(OnClickPause);
		CurrencyManager.OnCurrencyChange -= RefreshCurrencyAmount;
	}

	private void OnClickPause ()
	{
		GameManager.Instance.TogglePause();
	}

	private void RefreshCurrencyAmount ( Currency.Type currencyType )
	{
		m_currencyUIDictionary[currencyType].SetDisplay(CurrencyManager.GetCurrency(currencyType));
	}
}

[System.Serializable]
public class CurrencyUIPair
{
	public Currency.Type currencyType;
	public CurrencyUI currencyUI;
}
