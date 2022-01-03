using UnityEngine;

public class TowerEntity : MonoBehaviour
{
	public ulong woodPrice;
	public Sprite sprite => m_interactableTower.spriteRenderer.sprite;
	public SpriteRenderer spriteRenderer => m_interactableTower.spriteRenderer;
	[SerializeField] private InteractableTower m_interactableTower;

	private Vector3 m_lastTowerValidPosition;
	[HideInInspector] public bool towerPlaced = false;

	private void Awake ()
	{
		m_interactableTower.SetupTower(this);
		m_interactableTower.SetGhostMode();
	}

	public void OnTowerPlaced ()
	{
		if (!towerPlaced)
		{
			towerPlaced = true;
			CurrencyManager.RemoveCurrency(Currency.Type.Wood, woodPrice);
		}

		m_lastTowerValidPosition = transform.position;
		m_interactableTower.OnPlaced();
	}

	public void OnTowerDropCanceled ()
	{
		if (towerPlaced)
		{
			transform.position = m_lastTowerValidPosition;
			m_interactableTower.OnPlaced();
		}
	}
}