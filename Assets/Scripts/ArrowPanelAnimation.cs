using UnityEngine;

public class ArrowPanelAnimation : MonoBehaviour
{
	public bool StartAnimation;

	public float AnimationSpeed = 1f;

	public bool ToLeft;

	public float offset;

	private void FixedUpdate()
	{
		if (StartAnimation)
		{
			offset = Time.time * AnimationSpeed;
			if (ToLeft)
			{
				offset *= -1f;
			}
			Material material = GetComponent<Renderer>().material;
			float x = offset;
			Vector2 mainTextureOffset = GetComponent<Renderer>().material.mainTextureOffset;
			material.mainTextureOffset = new Vector2(x, mainTextureOffset.y);
			if (offset >= 10f || offset <= -10f)
			{
				offset = 0f;
			}
		}
	}
}
