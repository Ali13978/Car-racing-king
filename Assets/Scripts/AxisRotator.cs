using UnityEngine;

public class AxisRotator : MonoBehaviour
{
	public Vector3 Axis;

	public float smooth = 1f;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(Axis * Time.deltaTime * smooth);
	}
}
