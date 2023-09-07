using UnityEngine;

namespace RacingGameKit.Helpers
{
	[AddComponentMenu("")]
	public class GizmoHelper : MonoBehaviour
	{
		public Color GizmoColor = Color.yellow;

		public float GizmoRadius = 0.08f;

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = GizmoColor;
			Gizmos.DrawWireSphere(base.transform.position, GizmoRadius);
		}
	}
}
