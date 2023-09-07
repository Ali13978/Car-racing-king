using UnityEngine;

public class scrollUv : MonoBehaviour
{
	public Vector3 uvAnimationRate = new Vector3(1f, 0f, 1f);

	public string textureName = "_MainTex";

	public Material mat;

	private Vector3 uvOffset = Vector3.zero;

	private void Start()
	{
		if (mat == null)
		{
			mat = GetComponent<Renderer>().material;
		}
	}

	private void LateUpdate()
	{
		uvOffset += uvAnimationRate * Time.deltaTime;
		mat.SetTextureOffset(textureName, uvOffset);
	}
}
