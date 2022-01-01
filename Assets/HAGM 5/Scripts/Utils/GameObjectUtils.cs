using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
	public static void SetStaticRecursively (GameObject obj)
	{
		if (null == obj)
			return;

		obj.isStatic = true;

		foreach (Transform child in obj.transform)
		{
			if (null == child)
				continue;

			SetStaticRecursively(child.gameObject);
		}
	}
}
