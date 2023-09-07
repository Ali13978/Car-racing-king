using System;
using System.Collections.Generic;
using UnityEngine;

namespace EarthFX
{
	[AddComponentMenu("Racing Game Kit/EarthFX/EarthFX Manager")]
	public class EarthFX_Manager : MonoBehaviour
	{
		[Serializable]
		public class EarthFXData
		{
			public bool Visible = true;

			public string FxName = string.Empty;

			public Texture FxTexture;

			public GameObject Splatter;

			public float SplatterStartVelocity = 10f;

			public GameObject BrakeSkid;

			public GameObject BrakeSmoke;

			public float BrakeSkidStartSlip = 0.2f;

			public GameObject TrailSkid;

			public GameObject TrailSmoke;

			public float TrailSmokeStartVelocity = 15f;

			public AudioClip SurfaceDriveSound;

			public AudioClip BrakeSound;

			public bool EnableSpeedDeceleration;

			public float ForwardDrag;

			public float AngularDrag;

			public bool EnableGripDecrease;

			public float ForwardGripLosePercent;

			public float SidewaysGripLosePercent;

			public object Clone()
			{
				return MemberwiseClone();
			}
		}

		[HideInInspector]
		[SerializeField]
		public EarthFXData GlobalFX;

		[HideInInspector]
		[SerializeField]
		public List<EarthFXData> SurfaceFX;

		private void Awake()
		{
			GlobalFX.FxName = "GlobalFX";
		}
	}
}
