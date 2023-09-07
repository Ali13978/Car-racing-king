using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Start Finish/Finish Point")]
	[ExecuteInEditMode]
	public class FinishPointItem : ItemBase
	{
		private void OnDrawGizmos()
		{
			Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_finish.png");
		}
	}
}
