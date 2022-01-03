using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : Singleton<TreeManager>
{
	[SerializeField] private TreeEntity m_treePrefab;
	[SerializeField] private float m_spawnRadius;
	[SerializeField] private List<TreeEntity> m_treeList = new List<TreeEntity>();

	public override void Awake ()
	{
		base.Awake();
		TreeEntity.onTreeDestroyed += RemoveAndSpawnTree;
	}

	private void OnDestroy ()
	{
		TreeEntity.onTreeDestroyed -= RemoveAndSpawnTree;
	}
	#if UNITY_EDITOR
	private void OnDrawGizmosSelected ()
	{
		Gizmos.DrawWireSphere(transform.position, m_spawnRadius);
	}
	#endif

	private void RemoveAndSpawnTree ( TreeEntity treeEntity )
	{
		if (m_treeList.Contains(treeEntity))
		{
			m_treeList.Remove(treeEntity);
			Vector2 randomPos = Random.insideUnitCircle;
			TreeEntity newTree = Instantiate(m_treePrefab, GameManager.playerController.transform.position + new Vector3(randomPos.x, 0, randomPos.y) * m_spawnRadius, Quaternion.identity, transform);
			newTree.SpawnAnimation();
			m_treeList.Add(newTree);
		}
	}
}