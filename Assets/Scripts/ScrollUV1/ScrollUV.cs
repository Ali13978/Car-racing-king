using System;
using UnityEngine;

[Serializable]
public class ScrollUV : MonoBehaviour
{
	public float scrollSpeed_X;

	public float scrollSpeed_Y;

	public ScrollUV()
	{
		scrollSpeed_X = 0.5f;
		scrollSpeed_Y = 0.5f;
	}

	public void Update()
	{
		float x = Time.time * scrollSpeed_X;
		float y = Time.time * scrollSpeed_Y;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x, y);
	}

	public void Main()
	{
	}
}
