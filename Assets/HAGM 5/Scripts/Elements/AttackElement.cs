using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackElement : ScriptableObject
{
	[Tooltip("The elements that are defeated by this element.")]
	public List<AttackElement> defeatAbleElements = new List<AttackElement>();
}
