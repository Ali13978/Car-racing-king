using System;
using UnityEngine;

[Serializable]
public class RainDrawerContainer<T> where T : Component
{
	public T Drawer;

	public Transform transform;

	public RainDrawerContainer(string name, Transform parent)
	{
		transform = RainDropTools.CreateHiddenObject(name, parent);
		Drawer = transform.gameObject.AddComponent<T>();
	}
}
