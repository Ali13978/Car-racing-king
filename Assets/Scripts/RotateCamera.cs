using UnityEngine;

public class RotateCamera : MonoBehaviour
{
	public Transform target;

	public float speedMod = 3f;

	private Vector3 point;

	private void Start()
	{
		point = target.transform.position;
		base.transform.LookAt(point);
	}

	private void Update()
	{
		base.transform.RotateAround(point, new Vector3(0f, 1f, 0f), 20f * Time.deltaTime * speedMod);
	}
}
