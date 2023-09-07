using UnityEngine;

public class LookAtCameraBehaviour : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (!(Camera.current == null))
		{
			Transform transform = base.transform;
			Vector3 position = Camera.current.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			float y = position2.y;
			Vector3 position3 = Camera.current.transform.position;
			transform.LookAt(new Vector3(x, y, position3.z));
		}
	}
}
