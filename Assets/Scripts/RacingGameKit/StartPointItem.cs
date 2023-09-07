using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Start Finish/Start Point")]
	[ExecuteInEditMode]
	public class StartPointItem : MonoBehaviour
	{
		private void OnDrawGizmos()
		{
			Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_start.png");
		}
	}
}
