using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private PlayerMovement m_move;

	private void Reset ()
	{
		m_move = GetComponent<PlayerMovement>();
	}

	public void MoveToInteract ( Vector3 position, Action callback )
	{
		m_move.MoveToPosition(position, callback);
	}

	public void RotateToPosition(Vector3 position)
	{
		m_move.RotateTowards(position);
	}
}
