using System;
using UnityEngine;

[Serializable]
public class StaticRainVariables
{
	public bool AutoStart = true;

	public bool FullScreen = true;

	public Color OverlayColor = Color.gray;

	public Texture OverlayTexture;

	public Texture NormalMap;

	[Range(0f, 15f)]
	public float fadeTime = 2f;

	public AnimationCurve FadeinCurve;

	[Range(0.01f, 20f)]
	public float SizeX;

	[Range(0.01f, 20f)]
	public float SizeY;

	[Range(-2f, 2f)]
	public float SpawnOffsetX;

	[Range(-2f, 2f)]
	public float SpawnOffsetY;

	[Range(0.05f, 200f)]
	public float DistortionValue;

	[Range(0f, 2f)]
	public float ReliefValue;

	[Range(0f, 2f)]
	public float Blur;

	[Range(0f, 5f)]
	public float Darkness;
}
