using UnityEngine;

public class Pop : MonoBehaviour
{
	public bool StartAnimation;

	public float AnimationSpeed = 1f;

	public bool ToLeft;

	public float offset;

	public float intTimer = 1f;

	private void Update()
	{
		if (StartAnimation)
		{
			intTimer -= Time.deltaTime;
			if (intTimer <= 0f)
			{
				intTimer = AnimationSpeed;
				offset += 0.125f;
				Material material = GetComponent<Renderer>().material;
				float x = offset;
				Vector2 mainTextureOffset = GetComponent<Renderer>().material.mainTextureOffset;
				material.mainTextureOffset = new Vector2(x, mainTextureOffset.y);
			}
		}
	}
}
