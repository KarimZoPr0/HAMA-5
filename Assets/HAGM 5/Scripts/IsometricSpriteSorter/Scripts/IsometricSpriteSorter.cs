using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode,
DefaultExecutionOrder(-1)]
public class IsometricSpriteSorter : MonoBehaviour
{

	[System.Serializable]
	public class Sprite
	{
		public Renderer renderer;
		public int orderInLayer;
		public Action onOrderChange;

		public void SetOrder ( int orderInLayer )
		{
			this.orderInLayer = orderInLayer;
			renderer.sortingOrder = orderInLayer;
			onOrderChange?.Invoke();
		}
	}


	[SerializeField] private List<IsometricSpriteSorter.Sprite> m_sprites = new List<Sprite>();
	[SerializeField] private float m_pivotOffsetY;
	[SerializeField] private float m_sortingThreshold = 0.025f;

	public int YStep { get; private set; }
	public float pivotOffset
	{
		set { m_pivotOffsetY = value; }
	}
	public static float currentCameraY;
	public static int sortingStep;

	private bool m_orderChanged = false;


	public void AddAllSprites ()
	{
		Renderer[] spriteRenderers = transform.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < spriteRenderers.Length; i++)
		{
			Renderer renderer = spriteRenderers[i];
			if (m_sprites.Find(x => x.renderer == renderer) == null)
			{
				ParticleSystemRenderer particleSystemRenderer = renderer.GetComponent<ParticleSystemRenderer>();
				if (particleSystemRenderer != null && !particleSystemRenderer.enabled)
					continue;

				Sprite sprite = new Sprite { renderer = renderer, orderInLayer = renderer.sortingOrder };
				IsometricSpriteOrder isometricSpriteOrder = renderer.gameObject.GetComponent<IsometricSpriteOrder>();
				if (isometricSpriteOrder == null)
					isometricSpriteOrder = renderer.gameObject.AddComponent<IsometricSpriteOrder>();
				isometricSpriteOrder.Init(sprite);
				m_sprites.Add(sprite);
			}
		}
	}

	private void Awake ()
	{
#if UNITY_EDITOR
		if (m_sprites.Count == 0)
			return;
#endif
		for (int i = m_sprites.Count - 1; i >= 0; i--)
		{
			Sprite sprite = m_sprites[i];
			if (sprite != null && sprite.renderer != null)
			{
				sprite.renderer.sortingOrder = sprite.orderInLayer;
				sprite.onOrderChange += OnOrderChange;
				IsometricSpriteOrder isometricSpriteOrder = sprite.renderer.gameObject.GetComponent<IsometricSpriteOrder>();
				isometricSpriteOrder?.Init(sprite);
			}
			else
			{
				m_sprites.RemoveAt(i);
#if UNITY_EDITOR
				Debug.LogError("Fix missing sprites", this);
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}
	}

	private void OnOrderChange ()
	{
		m_orderChanged = true;
	}

	private void OnEnable ()
	{
#if UNITY_EDITOR
		if (m_sprites == null || m_sprites.Count == 0)
			return;
#endif

		this.ComputeOrder();
	}

	private void LateUpdate ()
	{
#if UNITY_EDITOR
		if (m_sprites == null || m_sprites.Count == 0)
			return;

		if (m_sprites[0].renderer == null)
			return;
#endif

		if (!m_sprites[0].renderer.isVisible)
			return;

		this.ComputeOrder();
	}

	private void ComputeOrder ()
	{

		int step = 0;

		if (m_pivotOffsetY != 0f)
			step = -(int)(((this.transform.position.y + m_pivotOffsetY) - currentCameraY) / m_sortingThreshold);
		else
			step = -(int)((this.transform.position.y - currentCameraY) / m_sortingThreshold);

		if (!m_orderChanged && step == this.YStep)
			return;
		m_orderChanged = false;

		this.YStep = step;

		step = step * sortingStep;

		m_sprites.ForEach(x =>
	   {
		   x.renderer.sortingOrder = step + x.orderInLayer;
	   });
	}

#if UNITY_EDITOR

	private void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.blue;
		Vector2 pos = (Vector2)this.transform.position;

		pos.y += m_pivotOffsetY;

		float size = HandleUtility.GetHandleSize(pos);
		Vector2 p1 = pos + Vector2.left * size;
		Vector2 p2 = pos + Vector2.right * size;

		Gizmos.DrawLine(p1, p2);
	}

#endif

}
