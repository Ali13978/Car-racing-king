using System;
using UnityEngine;

[Serializable]
public class FrictionFlowRainVariables
{
	public bool AutoStart = true;

	public bool PlayOnce;

	public Color OverlayColor = Color.gray;

	public Texture NormalMap;

	public Texture OverlayTexture;

	public Texture2D FrictionMap;

	public float Duration = 1f;

	public float Delay;

	public int MaxRainSpawnCount = 30;

	[Range(-2f, 2f)]
	public float SpawnOffsetY;

	[Range(0f, 10f)]
	public float LifetimeMin = 0.6f;

	[Range(0f, 10f)]
	public float LifetimeMax = 1.4f;

	[Range(0f, 50f)]
	public int EmissionRateMax = 5;

	[Range(0f, 50f)]
	public int EmissionRateMin = 2;

	public AnimationCurve AlphaOverLifetime;

	[Range(0f, 20f)]
	public float SizeMinX = 0.75f;

	[Range(0f, 20f)]
	public float SizeMaxX = 0.75f;

	public AnimationCurve TrailWidth;

	[Range(0f, 200f)]
	public float DistortionValue;

	public AnimationCurve DistortionOverLifetime;

	[Range(0f, 2f)]
	public float ReliefValue;

	public AnimationCurve ReliefOverLifetime;

	[Range(0f, 20f)]
	public float Blur;

	public AnimationCurve BlurOverLifetime;

	[Range(0f, 5f)]
	public float Darkness;

	[Range(-40f, 40f)]
	public float InitialVelocity;

	[Range(-5f, 5f)]
	public float AccelerationMin = 0.06f;

	[Range(-5f, 5f)]
	public float AccelerationMax = 0.2f;
}
