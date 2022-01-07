using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_currencyText;
	[SerializeField] private Image m_currencyIcon;

	private Tweener m_punchScaleTweener;
	private Tweener m_numberTweener;

	private ulong m_oldAmount;
	
	private void OnDestroy ()
	{
		m_punchScaleTweener?.Kill();
		m_numberTweener?.Kill();
	}

	public void Init ( Currency.Type currencyType )
	{
		m_currencyIcon.sprite = CurrencyManager.GetConfig(currencyType).sprite;
		m_currencyText.text = 0.ToString();
	}

	public void SetDisplay ( ulong amountToSet )
	{
		m_punchScaleTweener?.Kill();
		m_numberTweener?.Kill();

		if (amountToSet != 0 && m_oldAmount < amountToSet)
		{
			AudioManager.PlaySfx("Bop");
			m_punchScaleTweener = m_currencyText.rectTransform.DOPunchScale(.2f * Vector3.one, 0.5f);
		}
		else
			m_currencyText.rectTransform.localScale = Vector3.one;

		m_currencyText.text = amountToSet.ToString();
		m_oldAmount = amountToSet;
	}
}
