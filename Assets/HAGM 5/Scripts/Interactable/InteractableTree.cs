using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTree : InteractableGameElement
{
	[Title("References")]
	[SerializeField] private SpriteRenderer m_spriteRenderer;

	private void Reset ()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void Interact ()
	{

	}

	protected override void OnStartHover ()
	{
		m_spriteRenderer.material.SetFloat("_GhostBlend", 0.8f);
	}

	protected override void OnExitHover ()
	{
		m_spriteRenderer.material.SetFloat("_GhostBlend", 0f);
	}
}
