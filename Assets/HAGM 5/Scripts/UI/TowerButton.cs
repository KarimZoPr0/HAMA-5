using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
	[SerializeField] private TowerEntity m_towerPrefab;
	[SerializeField] private Button m_button;

	[SerializeField] private TextMeshProUGUI m_costText;
	[SerializeField] private Image m_towerImage;

	private void Awake ()
	{
		m_button.onClick.AddListener(TryBuyTower);
		CurrencyManager.OnCurrencyChange += OnCurrencyChanged;
		Init();
	}

	private void OnDestroy ()
	{
		m_button.onClick.RemoveListener(TryBuyTower);
		CurrencyManager.OnCurrencyChange -= OnCurrencyChanged;
	}

	private void OnCurrencyChanged ( Currency.Type currencyType )
	{
		m_button.interactable = CurrencyManager.GetCurrency(Currency.Type.Wood) >= m_towerPrefab.woodPrice;
	}

	private void Init ()
	{
		m_costText.text = m_towerPrefab.woodPrice.ToString();
		m_towerImage.sprite = m_towerPrefab.sprite;
	}

	private void TryBuyTower ()
	{
		if (CurrencyManager.GetCurrency(Currency.Type.Wood) >= m_towerPrefab.woodPrice)
			TowerManager.Instance.StartDroppingTower(m_towerPrefab);
	}
}
