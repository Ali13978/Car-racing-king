using UnityEngine;

public class MouseInstantiate : MonoBehaviour
{
	public GameObject prefabToInstantiate;

	public float speed = 7f;

	public void Update()
	{
		if (Input.GetMouseButtonDown(0) && prefabToInstantiate != null)
		{
			Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
			GameObject gameObject = UnityEngine.Object.Instantiate(prefabToInstantiate, ray.origin, Quaternion.identity);
			if (gameObject.GetComponent<Rigidbody>() != null)
			{
				gameObject.GetComponent<Rigidbody>().velocity = ray.direction * speed;
			}
		}
	}
}
