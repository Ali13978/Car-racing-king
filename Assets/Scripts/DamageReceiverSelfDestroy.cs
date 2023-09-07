using UnityEngine;

[AddComponentMenu("")]
public class DamageReceiverSelfDestroy : MonoBehaviour
{
	public float DestroyAfter = 10f;

	public bool StartCountdown;

	private void Update()
	{
		if (StartCountdown)
		{
			DestroyAfter -= Time.deltaTime;
			if (DestroyAfter <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
