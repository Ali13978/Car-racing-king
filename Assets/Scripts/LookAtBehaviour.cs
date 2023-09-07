using UnityEngine;

public class LookAtBehaviour : MonoBehaviour
{
	public Transform Target;

	private void Start()
	{
	}

	private void Update()
	{
		if (Target != null)
		{
			base.transform.LookAt(Target);
		}
	}
}
