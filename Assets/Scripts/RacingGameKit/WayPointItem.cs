using UnityEngine;

namespace RacingGameKit
{
	[AddComponentMenu("Racing Game Kit/Waypoint/Waypoint Item")]
	[ExecuteInEditMode]
	public class WayPointItem : ItemBase
	{
		public float SoftBrakeSpeed;

		public float HardBrakeSpeed;

		[HideInInspector]
		[SerializeField]
		public bool SeperatedWiders;

		[HideInInspector]
		public float LeftWide = 2f;

		[HideInInspector]
		public float RightWide = 2f;

		[HideInInspector]
		public Vector3 LeftLine;

		[HideInInspector]
		public Vector3 RightLine;

		[HideInInspector]
		public bool ShowIconGizmo = true;

		public bool ShowLines = true;

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			if (!SeperatedWiders)
			{
				LeftWide = RightWide;
			}
			if (SoftBrakeSpeed > 0f && HardBrakeSpeed <= SoftBrakeSpeed + 10f)
			{
				HardBrakeSpeed = SoftBrakeSpeed + 10f;
			}
			if (SoftBrakeSpeed > 0f)
			{
				if (ShowIconGizmo)
				{
					Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_waypoint_b.png");
				}
			}
			else if (ShowIconGizmo)
			{
				Gizmos.DrawIcon(base.transform.position, "rgk_gizmo_waypoint_n.png");
			}
			if (ShowLines)
			{
				Gizmos.DrawSphere(base.transform.position, 0.25f);
				Vector3 point = base.transform.position + base.transform.right * RightWide;
				Quaternion rotation = base.transform.rotation;
				RightLine = Quaternion.AngleAxis(rotation.y, base.transform.up) * point;
				Quaternion rotation2 = base.transform.rotation;
				RightLine = Quaternion.AngleAxis(rotation2.x, base.transform.right) * point;
				Quaternion rotation3 = base.transform.rotation;
				RightLine = Quaternion.AngleAxis(rotation3.z, base.transform.right) * point;
				Vector3 point2 = base.transform.position + base.transform.right * LeftWide * -1f;
				Quaternion rotation4 = base.transform.rotation;
				LeftLine = Quaternion.AngleAxis(rotation4.y, base.transform.up) * point2;
				Quaternion rotation5 = base.transform.rotation;
				LeftLine = Quaternion.AngleAxis(rotation5.x, base.transform.right * -1f) * point2;
				Quaternion rotation6 = base.transform.rotation;
				LeftLine = Quaternion.AngleAxis(rotation6.z, base.transform.right * -1f) * point2;
				Gizmos.DrawLine(base.transform.position, RightLine);
				Gizmos.DrawLine(base.transform.position, LeftLine);
			}
		}
	}
}
