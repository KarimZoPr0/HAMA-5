using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyConfig", menuName = "Config/CurrencyConfigs", order = 3)]
public class CurrencyConfigs : ScriptableObject
{
	public List<CurrencyConfig> currencyConfigList;
}

[System.Serializable]
public class CurrencyConfig
{
	[Title("Currency Infos")]
	public string name;
	public Currency.Type type;
	public Sprite sprite;

	[Title("Currency Cap")]
	public bool useCap = false;
	[ShowIf("useCap")] public ulong currencyCap;
}
