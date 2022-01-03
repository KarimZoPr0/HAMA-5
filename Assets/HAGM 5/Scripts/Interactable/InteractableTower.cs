using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractableTower : InteractableGameElement
{
	[Title("References")]
	public SpriteRenderer spriteRenderer;
	[SerializeField] private BoxCollider m_boxCollider;
	[SerializeField] private NavMeshObstacle m_navMeshObstacle;

	[Title("Ghost Settings")]
	[SerializeField] private float m_ghostBlendValue = .8f;
	[SerializeField] private float m_noGhostBlendValue = 0f;

	private Tweener m_transformTween;

	private void Awake ()
	{
		m_interactable = false;
		m_boxCollider.isTrigger = false;
		m_navMeshObstacle.enabled = false;
		GameObjectUtils.RemoveStaticRecursively(m_tower.gameObject);
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy();
		m_transformTween?.Kill();
	}

	private TowerEntity m_tower;
	public void OnPlaced()
	{
		m_interactable = true;
		m_navMeshObstacle.enabled = true;
		m_boxCollider.enabled = true;
		GameObjectUtils.SetStaticRecursively(m_tower.gameObject);
		RemoveGhostMode();
	}

	public void SetupTower ( TowerEntity tower )
	{
		m_tower = tower;
	}	

	protected override void Interact ()
	{

	}

	protected override void TryToInteract ()
	{
		m_interactable = false;
		m_boxCollider.isTrigger = false;
		m_navMeshObstacle.enabled = false;
		GameObjectUtils.RemoveStaticRecursively(m_tower.gameObject);
		TowerManager.Instance.GrabTower(m_tower);
	}

	protected override IEnumerator InteractCR ()
	{
		throw new System.NotImplementedException();
	}

	protected override void OnStartHover ()
	{
		SetGhostMode();
	}

	protected override void OnExitHover ()
	{
		RemoveGhostMode();
	}

	public void SetGhostMode ()
	{
		spriteRenderer.material.SetFloat("_GhostBlend", m_ghostBlendValue);
	}

	public void RemoveGhostMode ()
	{
		spriteRenderer.material.SetFloat("_GhostBlend", m_noGhostBlendValue);
	}
}
