using RacingGameKit.RGKCar;
using System.Collections.Generic;
using UnityEngine;

namespace EarthFX
{
	public interface IEarthFX
	{
		Transform WheelTransform
		{
			get;
		}

		RaycastHit HitToSurface
		{
			get;
		}

		float SlipRatio
		{
			get;
		}

		float SlipVelo
		{
			get;
		}

		float AngularVelocity
		{
			get;
		}

		float Grip
		{
			get;
			set;
		}

		float SideGrip
		{
			get;
			set;
		}

		float DefinedGrip
		{
			get;
			set;
		}

		float DefinedSideGrip
		{
			get;
			set;
		}

		bool OnGround
		{
			get;
		}

		bool EarthFXEnabled
		{
			set;
		}

		int LastSkid
		{
			get;
			set;
		}

		int LastTrail
		{
			get;
			set;
		}

		string PreviousSurface
		{
			get;
			set;
		}

		bool SurfaceChanged
		{
			get;
			set;
		}

		GameObject SkidObject
		{
			get;
			set;
		}

		GameObject TrailObject
		{
			get;
			set;
		}

		RGKCar_Skidmarks SkidMark
		{
			get;
			set;
		}

		RGKCar_Skidmarks TrailMark
		{
			get;
			set;
		}

		ParticleSystem SkidSmoke
		{
			get;
			set;
		}

		ParticleSystem TrailSmoke
		{
			get;
			set;
		}

		ParticleSystem Splatter
		{
			get;
			set;
		}

		List<SfxCache> SurfaceFxCache
		{
			get;
			set;
		}
	}
}
