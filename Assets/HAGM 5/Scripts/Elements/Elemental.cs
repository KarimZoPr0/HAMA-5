using UnityEngine;

public class Elemental : MonoBehaviour 
{
	[SerializeField] private UnitHealth m_unitHealth;
	public bool isAlive => m_unitHealth == null ? false : m_unitHealth.isAlive;
	[Tooltip("Element represented by this elemental.")]
	public AttackElement element;
}