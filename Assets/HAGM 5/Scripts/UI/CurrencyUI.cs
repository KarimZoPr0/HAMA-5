using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_currencyText;
	[SerializeField] private Image m_currencyIcon;

	public void Init ( Currency.Type currencyType )
	{
		m_currencyIcon.sprite = CurrencyManager.GetConfig(currencyType).sprite;
	}

	public void SetDisplay ( ulong textDisplayed )
	{
		m_currencyText.text = textDisplayed.ToString();
	}
}
