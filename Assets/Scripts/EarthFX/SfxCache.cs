using System;
using UnityEngine;

namespace EarthFX
{
	[Serializable]
	public class SfxCache
	{
		public string FxName;

		public ParticleSystem Splatter;

		public ParticleSystem TrailSmoke;

		public ParticleSystem BrakeSmoke;

		public GameObject BrakeSkid;

		public GameObject TrailSkid;
	}
}
