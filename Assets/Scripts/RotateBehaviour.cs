using UnityEngine;

public class RotateBehaviour : MonoBehaviour
{
	public Vector3 RotationAmount;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(RotationAmount * Time.deltaTime);
	}
}
