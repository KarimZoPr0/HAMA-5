using UnityEngine;

[ExecuteInEditMode]
public class IsometricSpriteOrder : MonoBehaviour
{
	IsometricSpriteSorter.Sprite m_sprite;
	public int newOrderInLayer;

	public void Init ( IsometricSpriteSorter.Sprite sprite )
	{
		m_sprite = sprite;
		newOrderInLayer = sprite.orderInLayer;
	}

	private void Start ()
	{
		if (m_sprite == null)
		{
			Debug.LogWarning("I am not linked to a Sprite Sorter : " + this + " // " + transform.parent.name);
		}
	}

	private void Update ()
	{
		if (m_sprite != null && m_sprite.orderInLayer != newOrderInLayer)
		{
			m_sprite.SetOrder(newOrderInLayer);
		}
	}
}
