using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Racing Line/Racing Line Item")]
	[ExecuteInEditMode]
	public class RacingLineItem : ItemBase
	{
		[HideInInspector]
		public bool ShowIconGizmo = true;

		[HideInInspector]
		public Color GizmoColor = Color.green;

		private void OnDrawGizmos()
		{
			if (ShowIconGizmo)
			{
				Gizmos.DrawIcon(base.transform.position, "icon_racingline.tif");
			}
		}
	}
}
