using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
	private List<TowerEntity> m_towerList = new List<TowerEntity>();

	private TowerEntity m_currentTower;
	private TowerEntity m_CurrentTower
	{
		get => m_currentTower;
		set
		{
			m_currentTower = value;
		}
	}

	public void StartDroppingTower ( TowerEntity t )
	{
		InputManager.LockGrab();

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		bool hasHit = Physics.Raycast(ray, out hit);

		if (hasHit)
			m_currentTower = Instantiate(t, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.identity, transform);
	}

	public void GrabTower ( TowerEntity t )
	{
		InputManager.LockGrab();

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		bool hasHit = Physics.Raycast(ray, out hit);

		if (hasHit)
			m_currentTower = t;
	}

	private void Update ()
	{
		if (m_CurrentTower != null)
		{
			if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) && !InputManager.pointerIn)
			{
				CancelDroppingTower();
				return;
			}

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			bool hasHit = Physics.Raycast(ray, out hit);

			if (hasHit)
			{
				m_CurrentTower.transform.position = new Vector3(hit.point.x, 0, hit.point.z);

				if (hit.collider.CompareTag("Ground"))
				{
					m_CurrentTower.spriteRenderer.color = Color.white;

					if (Input.GetMouseButtonDown(0))
						DropDownTower();
				}
				else
				{
					m_CurrentTower.spriteRenderer.color = Color.red;
				}
			}
		}
	}

	public void DropDownTower ()
	{
		InputManager.ReleaseGrab();
		m_CurrentTower.spriteRenderer.color = Color.white;
		m_CurrentTower.OnTowerPlaced();
		m_CurrentTower = null;
	}

	public void CancelDroppingTower ()
	{
		if (m_CurrentTower != null)
		{
			InputManager.ReleaseGrab();
			m_CurrentTower.spriteRenderer.color = Color.white;

			if (m_currentTower.towerPlaced)
				m_CurrentTower.OnTowerDropCanceled();
			else
				Destroy(m_CurrentTower.gameObject);

			m_CurrentTower = null;
		}
	}
}
