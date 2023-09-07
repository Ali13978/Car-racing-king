using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Check Points/CheckPoint Item")]
	[ExecuteInEditMode]
	public class CheckPointItem : ItemBase
	{
		[HideInInspector]
		public float CheckpointTime;

		[HideInInspector]
		public float CheckpointBonus;

		[HideInInspector]
		public bool ShowIcons = true;

		[HideInInspector]
		public eCheckpointItemType ItemType = eCheckpointItemType.Checkpoint;

		private void OnDrawGizmos()
		{
			if (ShowIcons)
			{
				if (ItemType == eCheckpointItemType.Checkpoint)
				{
					Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_checkpoint_c.png");
				}
				if (ItemType == eCheckpointItemType.Sector)
				{
					Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_checkpoint_s.png");
				}
				if (ItemType == eCheckpointItemType.SpeedTrap)
				{
					Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_checkpoint_ca.png");
				}
			}
		}
	}
}
