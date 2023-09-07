using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Spawn Points/Spawn Point")]
	[ExecuteInEditMode]
	public class SpawnPointItem : ItemBase
	{
		private void OnDrawGizmos()
		{
			Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_spawn.png");
		}
	}
}
