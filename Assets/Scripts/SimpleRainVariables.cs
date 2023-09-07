using System;
using UnityEngine;

[Serializable]
public class SimpleRainVariables
{
	public bool AutoStart = true;

	public bool PlayOnce;

	public Color OverlayColor = Color.gray;

	public Texture NormalMap;

	public Texture OverlayTexture;

	public bool AutoRotate;

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

	[Range(0f, 20f)]
	public float SizeMinY = 0.75f;

	[Range(0f, 20f)]
	public float SizeMaxY = 0.75f;

	public AnimationCurve SizeOverLifetime;

	[Range(0f, 200f)]
	public float DistortionValue;

	public AnimationCurve DistortionOverLifetime;

	[Range(0f, 2f)]
	public float ReliefValue;

	public AnimationCurve ReliefOverLifetime;

	[Range(0f, 2f)]
	public float Blur;

	public AnimationCurve BlurOverLifetime;

	public AnimationCurve PosYOverLifetime;

	[Range(0f, 5f)]
	public float Darkness;
}
