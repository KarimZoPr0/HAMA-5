using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CurrencyManager
{
	private static Dictionary<Currency.Type, ulong> m_currencyDictionary;
	public static Action<Currency.Type> OnCurrencyChange;
	public static Dictionary<Currency.Type, CurrencyConfig> m_currencyConfigDictionary;

	private static bool m_init = false;
	public static void Init ()
	{
		if (m_init)
			return;

		m_init = true;
		m_currencyDictionary = new Dictionary<Currency.Type, ulong>();

		foreach (Currency.Type type in (Currency.Type[])Enum.GetValues(typeof(Currency.Type)))
		{
			if (!m_currencyDictionary.ContainsKey(type))
			{
				m_currencyDictionary.Add(type, 0);
				OnCurrencyChange?.Invoke(type);
			}
		}

		m_currencyConfigDictionary = GameManager.currencyConfigs.currencyConfigList.ToDictionary(c => c.type);
	}

	public static void AddCurrency ( Currency.Type currencyType, ulong amount )
	{
		if (!m_init)
			Init();

		m_currencyDictionary[currencyType] += amount;
		OnCurrencyChange?.Invoke(currencyType);
	}

	public static void RemoveCurrency ( Currency.Type currencyType, ulong amount )
	{
		if (!m_init)
			Init();

		m_currencyDictionary[currencyType] -= amount;
		OnCurrencyChange?.Invoke(currencyType);
	}

	public static void SetCurrency ( Currency.Type currencyType, ulong amount )
	{
		if (!m_init)
			Init();

		m_currencyDictionary[currencyType] = amount;
		OnCurrencyChange?.Invoke(currencyType);
	}

	public static ulong GetCurrency ( Currency.Type currencyType )
	{
		if (!m_init)
			Init();

		return m_currencyDictionary[currencyType];
	}

	public static CurrencyConfig GetConfig (Currency.Type currencyType)
	{
		if (!m_init)
			Init();

		return m_currencyConfigDictionary[currencyType];
	}
}

public class Currency
{
	public enum Type
	{
		Dye,
		Wood,
	}
}