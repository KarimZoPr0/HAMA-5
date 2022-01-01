using UnityEngine;
using UnityEngine.Events;
public class AnimationEvent : MonoBehaviour
{
	public UnityEvent[] UnityEvents;

	public void TriggerUnityEvent(int index)
	{
		UnityEvents[index].Invoke();
	}
}
