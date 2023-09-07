using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrictionFlowRainController : MonoBehaviour
{
	public enum DrawState
	{
		Playing,
		Disabled
	}

	[Serializable]
	public class FrictionFlowRainDrawerContainer : RainDrawerContainer<TrailRenderer>
	{
		public DrawState currentState = DrawState.Disabled;

		public float initRnd;

		public float posXDt;

		public float rnd1;

		public float fluctuationRate = 5f;

		public float acceleration = 0.1f;

		public Vector3 startPos;

		public float TimeElapsed;

		public float lifetime;

		public bool IsEnable => Drawer.material != null && Drawer.enabled;

		public FrictionFlowRainDrawerContainer(string name, Transform parent)
			: base(name, parent)
		{
		}
	}

	private int oldSpawnLimit;

	private bool isOneShot;

	private bool isWaitingDelay;

	private float oneShotTimeleft;

	private float timeElapsed;

	private float interval;

	public List<FrictionFlowRainDrawerContainer> drawers = new List<FrictionFlowRainDrawerContainer>();

	public FrictionFlowRainVariables Variables
	{
		get;
		set;
	}

	[HideInInspector]
	public int RenderQueue
	{
		get;
		set;
	}

	public Camera camera
	{
		get;
		set;
	}

	public float Alpha
	{
		get;
		set;
	}

	public bool NoMoreRain
	{
		get;
		set;
	}

	public RainDropTools.RainDropShaderType ShaderType
	{
		get;
		set;
	}

	public bool IsPlaying => drawers.FindAll((FrictionFlowRainDrawerContainer t) => t.currentState == DrawState.Disabled).Count != drawers.Count;

	public void Refresh()
	{
		foreach (FrictionFlowRainDrawerContainer drawer in drawers)
		{
			UnityEngine.Object.DestroyImmediate(drawer.Drawer.gameObject);
		}
		drawers.Clear();
		for (int i = 0; i < Variables.MaxRainSpawnCount; i++)
		{
			FrictionFlowRainDrawerContainer frictionFlowRainDrawerContainer = new FrictionFlowRainDrawerContainer("Friction Flow RainDrawer " + i, base.transform);
			frictionFlowRainDrawerContainer.currentState = DrawState.Disabled;
			drawers.Add(frictionFlowRainDrawerContainer);
		}
	}

	public void Play()
	{
		StartCoroutine(PlayDelay(Variables.Delay));
	}

	private IEnumerator PlayDelay(float delay)
	{
		float t = 0f;
		while (t <= delay)
		{
			isWaitingDelay = true;
			t += Time.deltaTime;
			yield return null;
		}
		isWaitingDelay = false;
		if (drawers.Find((FrictionFlowRainDrawerContainer x) => x.currentState == DrawState.Playing) == null)
		{
			for (int i = 0; i < drawers.Count; i++)
			{
				InitializeDrawer(drawers[i]);
				drawers[i].currentState = DrawState.Disabled;
			}
			isOneShot = Variables.PlayOnce;
			if (isOneShot)
			{
				oneShotTimeleft = Variables.Duration;
			}
		}
	}

	public void UpdateController()
	{
		if (Variables == null)
		{
			return;
		}
		CheckSpawnNum();
		if (NoMoreRain)
		{
			timeElapsed = 0f;
		}
		else if (isOneShot)
		{
			oneShotTimeleft -= Time.deltaTime;
			if (oneShotTimeleft > 0f)
			{
				CheckSpawnTime();
			}
		}
		else if (!isWaitingDelay)
		{
			CheckSpawnTime();
		}
		for (int i = 0; i < drawers.Count; i++)
		{
			UpdateInstance(drawers[i], i);
		}
	}

	private void CheckSpawnNum()
	{
		int num = Variables.MaxRainSpawnCount - drawers.Count();
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				FrictionFlowRainDrawerContainer frictionFlowRainDrawerContainer = new FrictionFlowRainDrawerContainer("Friction Flow RainDrawer " + (drawers.Count() + i), base.transform);
				frictionFlowRainDrawerContainer.currentState = DrawState.Disabled;
				drawers.Add(frictionFlowRainDrawerContainer);
			}
		}
		if (num < 0)
		{
			int num2 = -num;
			List<FrictionFlowRainDrawerContainer> list = drawers.FindAll((FrictionFlowRainDrawerContainer x) => x.currentState != DrawState.Playing).Take(num2).ToList();
			if (list.Count() < num2)
			{
				list.AddRange(drawers.FindAll((FrictionFlowRainDrawerContainer x) => x.currentState == DrawState.Playing).Take(num2 - list.Count()));
			}
			foreach (FrictionFlowRainDrawerContainer item in list)
			{
				item.Drawer.Clear();
				UnityEngine.Object.DestroyImmediate(item.Drawer.gameObject);
			}
			drawers.RemoveAll((FrictionFlowRainDrawerContainer x) => x.Drawer == null);
		}
	}

	private void CheckSpawnTime()
	{
		if (interval == 0f)
		{
			interval = Variables.Duration / (float)RainDropTools.Random(Variables.EmissionRateMin, Variables.EmissionRateMax);
		}
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= interval)
		{
			int num = (int)Mathf.Min(timeElapsed / interval, Variables.MaxRainSpawnCount - drawers.FindAll((FrictionFlowRainDrawerContainer x) => x.currentState == DrawState.Playing).Count());
			for (int i = 0; i < num; i++)
			{
				Spawn();
			}
			interval = Variables.Duration / (float)RainDropTools.Random(Variables.EmissionRateMin, Variables.EmissionRateMax);
			timeElapsed = 0f;
		}
	}

	private void Spawn()
	{
		FrictionFlowRainDrawerContainer frictionFlowRainDrawerContainer = drawers.Find((FrictionFlowRainDrawerContainer x) => x.currentState == DrawState.Disabled);
		if (frictionFlowRainDrawerContainer != null)
		{
			InitializeDrawer(frictionFlowRainDrawerContainer);
			frictionFlowRainDrawerContainer.currentState = DrawState.Playing;
		}
	}

	private float GetProgress(FrictionFlowRainDrawerContainer dc)
	{
		return dc.TimeElapsed / dc.lifetime;
	}

	private void InitializeDrawer(FrictionFlowRainDrawerContainer dc)
	{
		dc.TimeElapsed = 0f;
		dc.lifetime = RainDropTools.Random(Variables.LifetimeMin, Variables.LifetimeMax);
		dc.acceleration = RainDropTools.Random(Variables.AccelerationMin, Variables.AccelerationMax);
		dc.transform.localPosition = RainDropTools.GetSpawnLocalPos(base.transform, camera, 0f, Variables.SpawnOffsetY);
		dc.startPos = dc.transform.localPosition;
		dc.acceleration = RainDropTools.Random(Variables.AccelerationMin, Variables.AccelerationMax);
		if (dc.Drawer.material == null)
		{
			Material material = RainDropTools.CreateRainMaterial(ShaderType, RenderQueue);
			RainDropTools.ApplyRainMaterialValue(material, ShaderType, Variables.NormalMap, Variables.OverlayTexture, Variables.DistortionValue, Variables.OverlayColor, Variables.ReliefValue, Variables.Blur, Variables.Darkness);
			dc.Drawer.material = material;
		}
		dc.Drawer.widthCurve = Variables.TrailWidth;
		dc.Drawer.widthMultiplier = RainDropTools.Random(Variables.SizeMinX, Variables.SizeMaxX);
		dc.Drawer.textureMode = LineTextureMode.Stretch;
		dc.Drawer.Clear();
		dc.Drawer.enabled = false;
	}

	private void Shuffle<T>(List<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = RainDropTools.Random(0, num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	private KeyValuePair<Vector3, float> PickRandomWeightedElement(Dictionary<Vector3, float> dictionary)
	{
		List<KeyValuePair<Vector3, float>> list = dictionary.ToList();
		float firstVal = list[0].Value;
		if (list.FindAll((KeyValuePair<Vector3, float> t) => t.Value == firstVal).Count() == list.Count())
		{
			Shuffle(list);
			return list[0];
		}
		list.Sort((KeyValuePair<Vector3, float> x, KeyValuePair<Vector3, float> y) => x.Value.CompareTo(y.Value));
		return list.FirstOrDefault((KeyValuePair<Vector3, float> x) => x.Value == dictionary.Values.Max());
	}

	private Vector3 GetNextPositionWithFriction(FrictionFlowRainDrawerContainer dc, float downValue, int resolution, int widthResolution, float dt)
	{
		int width = Variables.FrictionMap.width;
		int height = Variables.FrictionMap.height;
		int num = (int)Mathf.Max(2f, (float)resolution * dt);
		Vector3 vector = dc.Drawer.transform.localPosition;
		Dictionary<Vector3, float> dictionary = new Dictionary<Vector3, float>();
		for (int i = 0; i < num; i++)
		{
			float d = downValue * ((float)i / (float)num);
			dictionary.Clear();
			for (int j = 0; j <= 2 * widthResolution; j++)
			{
				Vector3 a = vector + Vector3.down * d;
				a += Vector3.left * d + Vector3.right * d * ((float)j / (float)widthResolution);
				Vector3 vector2 = camera.WorldToViewportPoint(a);
				float grayscale = Variables.FrictionMap.GetPixel((int)((float)width * vector2.x), (int)((float)height * (0f - vector2.y))).grayscale;
				if (!dictionary.ContainsKey(a))
				{
					dictionary.Add(a, 1f - grayscale);
				}
			}
			Vector3 key = PickRandomWeightedElement(dictionary).Key;
			vector = key;
		}
		return vector;
	}

	private void UpdateTransform(FrictionFlowRainDrawerContainer dc)
	{
		float num = dc.TimeElapsed;
		Vector3 nextPositionWithFriction = GetNextPositionWithFriction(dc, 0.5f * num * num * dc.acceleration * 0.1f + Variables.InitialVelocity * num * 0.01f, 150, 8, Time.deltaTime);
		nextPositionWithFriction = new Vector3(nextPositionWithFriction.x, nextPositionWithFriction.y, 0f);
		dc.transform.localPosition = nextPositionWithFriction;
	}

	private void UpdateShader(FrictionFlowRainDrawerContainer dc, int index)
	{
		float progress = GetProgress(dc);
		dc.Drawer.material.renderQueue = RenderQueue + index;
		if (dc.Drawer.material.shader.name != RainDropTools.GetShaderName(ShaderType))
		{
			dc.Drawer.material = RainDropTools.CreateRainMaterial(ShaderType, RenderQueue + index);
		}
		float num = Variables.DistortionValue * Variables.DistortionOverLifetime.Evaluate(progress) * Alpha;
		float num2 = Variables.ReliefValue * Variables.ReliefOverLifetime.Evaluate(progress) * Alpha;
		float num3 = Variables.Blur * Variables.BlurOverLifetime.Evaluate(progress) * Alpha;
		Color value = new Color(Variables.OverlayColor.r, Variables.OverlayColor.g, Variables.OverlayColor.b, Variables.OverlayColor.a * Variables.AlphaOverLifetime.Evaluate(progress) * Alpha);
		switch (ShaderType)
		{
		case RainDropTools.RainDropShaderType.Expensive:
			if (num == 0f && num2 == 0f && value.a == 0f && num3 == 0f)
			{
				dc.Drawer.enabled = false;
				return;
			}
			break;
		case RainDropTools.RainDropShaderType.Cheap:
			if (num == 0f)
			{
				dc.Drawer.enabled = false;
				return;
			}
			break;
		case RainDropTools.RainDropShaderType.NoDistortion:
			if (num2 == 0f && value.a == 0f)
			{
				dc.Drawer.enabled = false;
				return;
			}
			break;
		}
		RainDropTools.ApplyRainMaterialValue(dc.Drawer.material, ShaderType, Variables.NormalMap, Variables.OverlayTexture, num, value, num2, num3, Variables.Darkness * Alpha);
		dc.Drawer.enabled = true;
	}

	private void UpdateInstance(FrictionFlowRainDrawerContainer dc, int index)
	{
		if (dc.currentState == DrawState.Playing)
		{
			if (GetProgress(dc) >= 1f)
			{
				dc.Drawer.Clear();
				dc.currentState = DrawState.Disabled;
			}
			else
			{
				dc.TimeElapsed += Time.deltaTime;
				UpdateTransform(dc);
				UpdateShader(dc, index);
			}
		}
	}
}
