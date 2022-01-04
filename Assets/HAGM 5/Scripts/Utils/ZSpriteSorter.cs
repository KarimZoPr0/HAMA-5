using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSpriteSorter : MonoBehaviour
{
	[SerializeField] private int m_sortingOrderBase = 0;
	[SerializeField] private int m_offset;
	[SerializeField] private bool m_runOnlyOnce = false;

	private float m_timer;
	private float m_timerMax = .1f;
	[SerializeField] private SpriteRenderer m_renderer;
	[SerializeField] private Transform m_pivotTransform;

	private void Reset ()
	{
		m_renderer = gameObject.GetComponent<SpriteRenderer>();
	}

	private void LateUpdate ()
	{
		m_timer -= Time.deltaTime;
		if (m_timer <= 0f)
		{
			m_timer = m_timerMax;
			m_renderer.sortingOrder = (int)(m_sortingOrderBase - (m_pivotTransform.position.z * 3f) - m_offset);
			if (m_runOnlyOnce)
				Destroy(this);
		}
	}
}
