using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("")]
	public class ItemBase : MonoBehaviour
	{
		private float ColliRadius = 0.2f;

		public void AlignToTerrain()
		{
			CastToCollider(forward: new Vector3(0f, -1f, 0f), fromPos: base.transform.position, minDistance: 0f, maxDistance: 0f);
		}

		public void CastToCollider(Vector3 fromPos, Vector3 forward, float minDistance, float maxDistance)
		{
			Ray ray = new Ray(fromPos, forward);
			bool flag = false;
			if ((!(maxDistance > 0f)) ? Physics.SphereCast(ray, ColliRadius, out RaycastHit hitInfo) : Physics.SphereCast(ray, ColliRadius, out hitInfo, maxDistance))
			{
				base.transform.position = hitInfo.point;
				base.transform.position += Vector3.up.normalized * 0.5f;
			}
			else if (minDistance > 0f)
			{
				base.transform.position = fromPos + forward.normalized * minDistance;
				base.transform.position += Vector3.up.normalized * 0.5f;
			}
		}
	}
}
